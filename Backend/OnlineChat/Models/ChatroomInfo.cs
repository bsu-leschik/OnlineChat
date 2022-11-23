namespace OnlineChat.Models;

public class ChatroomInfo
{
    public int Id { get; set; }
    public int UsersCount { get; set; }

    public static ChatroomInfo Of(Chatroom room)
    {
        return new ChatroomInfo
        { 
          Id = room.Id,
          UsersCount = room.Users.Count 
        };
    }
}