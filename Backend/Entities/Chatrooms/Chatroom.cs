using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Chatrooms;

public abstract class Chatroom : IEquatable<Chatroom>
{
    public Guid Id { get; set; }
    public List<ChatroomTicket> UserTickets { get; set; }

    [NotMapped] public IEnumerable<User> Users => UserTickets.Select(t => t.User);
    [NotMapped] public int UsersCount => UserTickets.Count;
    public List<Message> Messages { get; set; }
    public ChatType Type { get; }
    public DateTime LastMessageTime { get; set; }

    public Chatroom(Guid id, ChatType type, List<User> users)
    {
        if (users.Count == 0)
        {
            throw new ArgumentException($"{nameof(users)} contains no users");
        }
        
        Id = id;
        Type = type;
        Messages = new List<Message>();
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
        LastMessageTime = DateTime.Now;
    }

    public bool Equals(Chatroom? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals(obj as Chatroom);
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