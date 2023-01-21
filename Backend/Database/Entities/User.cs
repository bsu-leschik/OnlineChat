﻿namespace Database.Entities;

/// <summary>
/// Data class
/// </summary>
public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public List<Chatroom> Chatrooms { get; } = new();
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
        if (obj is not User user)
        {
            return false;
        }

        return user.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}