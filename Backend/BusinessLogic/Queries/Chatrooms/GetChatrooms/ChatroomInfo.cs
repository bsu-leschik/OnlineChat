using Database.Entities;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class ChatroomInfo
{
    public string Id { get; set; }
    public int UsersCount { get; set; }

    public static ChatroomInfo Of(Chatroom room)
    {
        return new ChatroomInfo
        { 
        Id = room.Id.ToString(),
        UsersCount = room.Users.Count 
        };
    }
}