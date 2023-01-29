using System.Diagnostics;
using Entities.Chatrooms;

namespace BusinessLogic.Models;

public static class ChatroomInfo
{
    public static object Of(Chatroom chatroom) => chatroom switch
        {
            PublicChatroom pc => PublicChatroomInfo.Of(pc),
            PrivateChatroom pr => PrivateChatroomInfo.Of(pr),
            _ => throw new UnreachableException()
        };
}

public class PrivateChatroomInfo
{
    public Guid Id { get; set; }
    public List<string> Users { get; set; }
    public ChatType Type { get; set; } = ChatType.Private;
    public DateTime LastMessageTime { get; set; }

    public static PrivateChatroomInfo Of(PrivateChatroom pc)
    {
        return new PrivateChatroomInfo
                   {
                       Id = pc.Id,
                       Users = pc.Users.Select(u => u.Username).ToList(),
                       LastMessageTime = pc.LastMessageTime
                   };
    }
}

public class PublicChatroomInfo
{
    public Guid Id { get; set; }
    public List<string> Users { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public List<string> Moderators { get; set; } = null!;
    public ChatType Type { get; set; } = ChatType.Public;
    public DateTime LastMessageTime { get; set; }

    public static PublicChatroomInfo Of(PublicChatroom pc)
    {
        return new PublicChatroomInfo
                   {
                       Id = pc.Id,
                       Users = pc.Users.Select(u => u.Username).ToList(),
                       Owner = pc.Administrators.Owner.Username,
                       Moderators = pc.Administrators.Moderators.Select(u => u.Username).ToList(),
                       LastMessageTime = pc.LastMessageTime
                   };
    }
}