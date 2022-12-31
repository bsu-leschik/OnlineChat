using Database.Entities;

namespace BusinessLogic.Services;

public class Storage// : IStorageService
{
    private int _chatroomCount = 0;
    private readonly List<Chatroom> _chatrooms = new();
    private readonly List<User> _users = new();

    public Storage()
    {
        CreateNewChatroom();
    }

    public Chatroom CreateNewChatroom()
    {
        return null;
    }

    // var lastId = _chatrooms.LastOrDefault()?.Id ?? 0;
    // if (lastId == int.MaxValue)
    // {
    // RemapChatrooms();
    // CreateNewChatroom();
    // }
    // var chatroom = new Chatroom(_chatroomCount);
    // TODO: fix
    // lock (_chatrooms)
    // {
    // _chatrooms.Add(chatroom);
    // }

    // ++_chatroomCount;
    // return chatroom;
    // }

    public Task<User?> GetUser(Func<User, bool> predicate)
    {
        lock (_users)
        {
            return Task.FromResult(_users.FirstOrDefault(predicate));
        }
    }

    public Task<Chatroom?> GetChatroom(Func<Chatroom, bool> predicate)
    {
        return Task.FromResult(_chatrooms.FirstOrDefault(predicate));
    }

    public Task AddChatroom(Chatroom chatroom)
    {
        _chatrooms.Add(chatroom);
        return Task.CompletedTask;
    }

    public Task AddUser(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task Remove(User u)
    {
        lock (_users)
        {
            lock (_chatrooms)
            {
                _users.Remove(u);
                var chatroom = _chatrooms.FirstOrDefault(c => c.Users.Contains(u));
                if (chatroom is null)
                {
                    return Task.CompletedTask;
                }

                chatroom.Users.Remove(u);
            }
        }
        return Task.CompletedTask;
    }

    public Task Remove(Chatroom chatroom)
    {
        _chatrooms.Remove(chatroom);
        return Task.CompletedTask;
    }

    public Task<List<Chatroom>> GetChatrooms()
    {
        return Task.FromResult(_chatrooms);
    }
}