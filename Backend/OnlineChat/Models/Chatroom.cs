namespace OnlineChat.Models;

public class Chatroom
{
    public int Id { get; set; }
    public List<User> Users { get; } = new();
    public List<Message> Messages { get; } = new();
    public DateTime LastEmptyTime { get; set; } = DateTime.Now;
    public Chatroom(int id)
    {
        Id = id;
    }

    public bool IsEmpty()
    {
        return Users.Count == 0;
    }
}