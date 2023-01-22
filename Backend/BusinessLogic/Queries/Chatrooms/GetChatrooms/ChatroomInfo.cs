using Entities.Chatrooms;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class ChatroomInfo
{
    public string Id { get; set; }
    public int UsersCount { get; set; }
    public List<string> Users { get; set; }
    public ChatType ChatType { get; set; }

    public static ChatroomInfo Of(Chatroom room)
    {
        return new ChatroomInfo
        { 
            Id = room.Id.ToString(),
            UsersCount = room.Users.Count,
            Users = room.Users.Select(user => user.Username).ToList(),
            ChatType = room.Type
        };
    }
}