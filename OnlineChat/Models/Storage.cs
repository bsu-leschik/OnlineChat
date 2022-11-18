namespace OnlineChat.Models;

public class Storage
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
        var chatroom = new Chatroom(_chatroomCount);
        _chatrooms.Add(chatroom);
        ++_chatroomCount;
        return chatroom;
    }

    public Chatroom? GetChatroomById(int id)
    {
        return _chatrooms.FirstOrDefault(c => c.Id == id);
    }

    public void AddUser(User u, int chatroomId)
    {
        _users.Add(u);
    }

    public User? GetUser(string connectionId)
    {
        return _users.FirstOrDefault(u => u.ConnectionId == connectionId);
    }

    public void Remove(User u)
    {
        _users.Remove(u);
        _chatrooms.FirstOrDefault(c => c.Users.Contains(u))?.Users.Remove(u);
    }
}