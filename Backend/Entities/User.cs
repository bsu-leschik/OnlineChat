using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Chatrooms;

namespace Entities;

/// <summary>
/// Data class
/// </summary>
public class User : IEquatable<User>
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<ChatroomTicket> ChatroomTickets { get; set; } = new();
    [NotMapped] public IEnumerable<Chatroom> Chatrooms => ChatroomTickets.Select(t => t.Chatroom);
    public string Role { get; set; } = "User";
    public Guid Token { get; set; } = Guid.NewGuid();

    public void UpdateToken()
    {
        Token = Guid.NewGuid();
    }

    public User(string username, string password)
    {
        Id = Guid.NewGuid();
        Username = username;
        Password = password;
    }

    public User() {}

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((User) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Username);
    }

    public bool Equals(User? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Username == other.Username;
    }

    public static bool operator ==(User a, User b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(User a, User b)
    {
        return !(a == b);
    }

    public bool Leave(Chatroom c)
    {
        var ticket = ChatroomTickets.FirstOrDefault(t => t.Chatroom == c);
        return ticket is not null && ChatroomTickets.Remove(ticket);
    }

    public void Join(Chatroom chatroom)
    {
        var ticket = new ChatroomTicket(this, chatroom);
        ChatroomTickets.Add(ticket);
        chatroom.UserTickets.Add(ticket);
    }
}