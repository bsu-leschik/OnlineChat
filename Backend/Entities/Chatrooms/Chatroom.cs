using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Chatrooms;

public abstract class Chatroom : IEquatable<Chatroom>
{
    public Guid Id { get; set; }
    public List<ChatroomTicket> UserTickets { get; set; }

    [NotMapped] public IEnumerable<User> Users => UserTickets.Select(t => t.User);
    [NotMapped] public int UsersCount => UserTickets.Count;
    public int MessagesCount { get; set; }
    public ChatType Type { get; }
    public DateTime LastMessageTime { get; set; }
    public List<Message> Messages { get; internal set; }
    private static Message ChatCreatedMessage => new Message("", "The chat was created");
    public Chatroom(Guid id, ChatType type, List<User> users)
    {
        if (users.Count == 0)
        {
            throw new ArgumentException($"{nameof(users)} contains no users");
        }
        
        Id = id;
        Type = type;
        MessagesCount = 0;
        Messages = new List<Message> { ChatCreatedMessage };
        LastMessageTime = DateTime.Now;
        UserTickets = users.Select(u => new ChatroomTicket(u, this)).ToList();
    }

    protected Chatroom() {}

    public bool IsEmpty()
    {
        return UserTickets.Count == 0;
    }

    public void AddMessage(Message message)
    {
        Messages.Add(message);
        ++MessagesCount;
        LastMessageTime = DateTime.Now;
    }

    public bool Equals(Chatroom? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Chatroom);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Chatroom a, Chatroom b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Chatroom a, Chatroom b)
    {
        return !(a == b);
    }
}