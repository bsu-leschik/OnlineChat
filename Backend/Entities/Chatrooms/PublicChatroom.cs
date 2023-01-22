using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Chatrooms;

[Table(nameof(PublicChatroom))]
public class PublicChatroom : Chatroom
{
    public string Name { get; set; }

    private Guid OwnerId { get; set; }

    [NotMapped]
    public User Owner
    {
        get => Users.First(u => u.Id == OwnerId);
        set => OwnerId = value.Id;
    }

    private List<string> ModeratorsNicknames { get; }

    [NotMapped] public IEnumerable<User> Moderators => Users.Where(u => ModeratorsNicknames.Contains(u.Username));

    public PublicChatroom(Guid id, List<User> users, User owner, string name) : base(id, ChatType.Public, users)
    {
        if (!users.Contains(owner))
        {
            throw new ArgumentException("owner must be in chat");
        }
        ModeratorsNicknames = new List<string>();
        OwnerId = owner.Id;
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
            ModeratorsNicknames.Remove(user.Username);
        }
        Users.Remove(user);
        user.Leave(this);
    }

    public void AddModerator(User user)
    {
        ModeratorsNicknames.Add(user.Username);
    }

    public void RemoveModerator(User user)
    {
        ModeratorsNicknames.Remove(user.Username);
    }
}