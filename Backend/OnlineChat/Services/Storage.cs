using System.Collections.Immutable;
using OnlineChat.Models;
using OnlineChat.Services.StorageSanitizer;

namespace OnlineChat.Services;

public class Storage : IDisposable
{
    private int _chatroomCount = 0;
    private readonly List<Chatroom> _chatrooms = new();
    private readonly List<User> _users = new();
    private readonly List<IStorageSanitizer> _sanitizers = new();

    public Storage()
    {
        CreateNewChatroom();
    }
    
    public Chatroom CreateNewChatroom()
    {
        int lastId = _chatrooms.LastOrDefault()?.Id ?? 0; 
        if (lastId  == int.MaxValue)
        {
            RemapChatrooms();
            CreateNewChatroom();
        }
        var chatroom = new Chatroom(_chatroomCount);
        lock (_chatrooms)
        {
            _chatrooms.Add(chatroom);
        }
        ++_chatroomCount;
        return chatroom;
    }

    public Chatroom? GetChatroomById(int id)
    {
        lock (_chatrooms)
        {
            return _chatrooms.FirstOrDefault(c => c.Id == id);
        }
    }

    public void AddUser(User u, int chatroomId)
    {
        lock (_users)
        {
            _users.Add(u);
            var chatroom = GetChatroomById(chatroomId);
            if (chatroom is null)
            {
                return;
            }

            if (chatroom.IsEmpty())
            {
                chatroom.LastEmptyTime = DateTime.Now;
            }
            chatroom.Users.Add(u);
        }
    }

    public int RemoveChatrooms(Predicate<Chatroom> predicate)
    {
        lock (_chatrooms)
        {
            return _chatrooms.RemoveAll(predicate);
        }
    }

    public void AddStorageSanitizer(IStorageSanitizer sanitizer)
    {
        _sanitizers.Add(sanitizer);
        sanitizer.StartSanitizing();
    }

    public void RemoveStorageSanitizer(IStorageSanitizer sanitizer)
    {
        _sanitizers.Remove(sanitizer);
        if (sanitizer is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private void RemapChatrooms()
    {
        lock (_chatrooms)
        {
            for (var i = 0; i < _chatrooms.Count; ++i)
            {
                _chatrooms[i].Id = i;
            }

            _chatroomCount = _chatrooms.Count;
        }
    }
    
    public User? GetUser(string connectionId)
    {
        lock (_users)
        {
            return _users.FirstOrDefault(u => u.ConnectionId == connectionId);
        }
    }

    public void Remove(User u)
    {
        lock (_users)
        {
            lock (_chatrooms)
            {
                _users.Remove(u);
                var chatroom = _chatrooms.FirstOrDefault(c => c.Users.Contains(u));
                if (chatroom is null)
                {
                    return;
                }

                chatroom.Users.Remove(u);
                if (chatroom.IsEmpty())
                {
                    chatroom.LastEmptyTime = DateTime.Now;
                }
            }
        }
    }

    public ImmutableList<Chatroom> GetChatrooms()
    {
        lock (_chatrooms)
        {
            return _chatrooms.ToImmutableList();
        }
    }

    public void Dispose()
    {
        foreach (var sanitizer in _sanitizers.OfType<IDisposable>())
        {
            sanitizer.Dispose();
        }
    }
}