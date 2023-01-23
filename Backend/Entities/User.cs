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
    public List<Chatroom> Chatrooms { get; set; } = new();
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
    }

    public void Join(Chatroom chatroom)
    {
        Chatrooms.Add(chatroom);
    }
}