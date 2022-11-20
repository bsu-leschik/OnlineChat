namespace OnlineChat.Models;

public class Chatroom
{
    public int Id { get; }
    public List<User> Users { get; }= new();
    public List<Message> Messages { get; } = new();

    public Chatroom(int id)
    {
        Id = id;
    }
}