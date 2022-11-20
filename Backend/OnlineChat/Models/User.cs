namespace OnlineChat.Models;

public class User
{
    public string Nickname { get; set; }
    public string ConnectionId { get; }
    public static readonly int NotInChatroom = -1;
    public int ChatroomId { get; set; }

    public User(string nickname, string connectionId, int chatroomId)
    {
        Nickname = nickname;
        ConnectionId = connectionId;
        ChatroomId = chatroomId;
    }
}