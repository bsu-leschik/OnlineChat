namespace OnlineChat.Models;

public class User
{
    public string Nickname { get; set; }
    public string ConnectionId { get; }
    public static readonly int NotInChatroom = -1;
    public int ChatroomId { get; set; }
    public DateTime LastMessageTime { get; private set; } = DateTime.MinValue;

    public void ResetLastMessageTime()
    {
        LastMessageTime = DateTime.Now;
    }

    public User(string nickname, string connectionId, int chatroomId)
    {
        Nickname = nickname;
        ConnectionId = connectionId;
        ChatroomId = chatroomId;
    }
}