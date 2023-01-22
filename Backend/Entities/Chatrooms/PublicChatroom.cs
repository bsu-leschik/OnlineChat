namespace Entities.Chatrooms;

public class PublicChatroom : Chatroom
{
    public string Name { get; set; }
    public User Owner { get; set; }
    public List<User> Moderators { get; set; }

    public PublicChatroom(Guid id, List<User> users, User owner, string name) : base(id, ChatType.Public, users)
    {
        if (!users.Contains(owner))
        {
            throw new ArgumentException("owner must be in chat");
        }
        Moderators = new List<User>();
        Owner = owner;
        Name = name;
    }

    public PublicChatroom() {}

    public bool Kick(User user)
    {
        if (!Users.Contains(user) || user == Owner)
        {
            return false;
        }
        
        ForceKick(user);
        return true;
    }

    public void ForceKick(User user)
    {
        if (Moderators.Contains(user))
        {
            Moderators.Remove(user);
        }
        Users.Remove(user);
        user.Chatrooms.Remove(this);
    }
}