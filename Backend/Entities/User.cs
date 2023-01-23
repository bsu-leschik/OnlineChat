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
    // public List<PublicChatroom> PublicChatrooms { get; } = new();
    // public List<PrivateChatroom> PrivateChatrooms { get; } = new();

    // [NotMapped] public IEnumerable<Chatroom> Chatrooms => PublicChatrooms.Cast<Chatroom>().Union(PrivateChatrooms);
    public List<Chatroom> Chatrooms { get; set; }
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
        return Chatrooms.Remove(c);
        // return c.Type == ChatType.Private
        // ? PrivateChatrooms.Remove((c as PrivateChatroom)!)
        // : PublicChatrooms.Remove((c as PublicChatroom)!);
    }

    public void Join(Chatroom chatroom)
    {
        // if (chatroom.Type == ChatType.Private)
        // {
            // PrivateChatrooms.Add((chatroom as PrivateChatroom)!);
        // }
        // else
        // {
            // PublicChatrooms.Add((chatroom as PublicChatroom)!);
        // }
        Chatrooms.Add(chatroom);
    }
}