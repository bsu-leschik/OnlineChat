using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Chatrooms;

[Table(nameof(PublicChatroom))]
public class PublicChatroom : Chatroom
{
    public string Name { get; set; }
    public Administrators Administrators { get; set; }

    public PublicChatroom(Guid id, List<User> users, User owner, string name) : base(id, ChatType.Public, users)
    {
        if (!users.Contains(owner))
        {
            throw new ArgumentException("owner must be in chat");
        }
        Administrators = new Administrators(owner: owner, moderators: new List<User>(), this);
        Name = name;
    }

    public PublicChatroom() {}

    public bool Kick(User user)
    {
        if (!Users.Contains(user) || user == Administrators.Owner)
        {
            return false;
        }

        ForceKick(user);
        return true;
    }

    public void ForceKick(User user)
    {
        if (Administrators.Moderators.Contains(user))
        {
            Administrators.Moderators.Remove(user);
        }
        Users.Remove(user);
        user.Leave(this);
    }

    public void AddModerator(User user)
    {
        Administrators.Moderators.Add(user);
    }

    public void RemoveModerator(User user)
    {
        Administrators.Moderators.Add(user);
    }
}

public class Administrators
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PublicChatroomId { get; set; }
    public User Owner { get; set; }

    public List<User> Moderators { get; set; }
    public PublicChatroom PublicChatroom { get; set; }

    public Administrators() {}

    public Administrators(User owner, List<User> moderators, PublicChatroom chatroom)
    {
        Owner = owner;
        Moderators = moderators;
        PublicChatroom = chatroom;
    }
}