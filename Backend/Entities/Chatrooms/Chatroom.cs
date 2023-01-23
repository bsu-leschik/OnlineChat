using System.Collections.Immutable;

namespace Entities.Chatrooms;

public abstract class Chatroom
{
    public Guid Id { get; set; }
    public List<User> Users { get; set; }
    public List<Message> Messages { get; set; }
    public ChatType Type { get; }
    public DateTime LastMessageTime { get; set; }

    public Chatroom(Guid id, ChatType type, List<User> users)
    {
        Id = id;
        Type = type;
        Messages = new List<Message>();
        LastMessageTime = DateTime.Now;
        Users = new List<User>(
            type == ChatType.Private
                ? users.ToImmutableList()
                : users);
        if (users.Count == 0)
        {
            throw new ArgumentException($"{nameof(users)} contains no users");
        }

        if (type == ChatType.Private && users.Count != 2)
        {
            throw new ArgumentException($"Private chat must contain only 2 members");
        }
    }

    public Chatroom() {}

    public bool IsEmpty()
    {
        return Users.Count == 0;
    }

    public void AddMessage(Message message)
    {
        Messages.Add(message);
        LastMessageTime = DateTime.Now;
    }
}