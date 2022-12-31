using Database.Entities;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class ChatroomInfo
{
    public Guid Id { get; set; }
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