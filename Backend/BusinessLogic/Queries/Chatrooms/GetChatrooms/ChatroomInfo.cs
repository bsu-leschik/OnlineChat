﻿using Entities;
using Entities.Chatrooms;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class PrivateChatroomInfo
{
    public List<string> Users { get; set; }
    public ChatType Type { get; set; } = ChatType.Private;
    public DateTime LastMessageTime { get; set; }

    public static PrivateChatroomInfo Of(PrivateChatroom pc)
    {
        return new PrivateChatroomInfo
                   {
                       Users = pc.Users.Select(u => u.Username).ToList(), LastMessageTime = pc.LastMessageTime
                   };
    }
}

public class PublicChatroomInfo
{
    public List<string> Users { get; set; } = null!;
    public string Owner { get; set; } = null!;
    public List<string> Moderators { get; set; } = null!;
    public ChatType Type { get; set; } = ChatType.Public;
    public DateTime LastMessageTime { get; set; }

    public static PublicChatroomInfo Of(PublicChatroom pc)
    {
        return new PublicChatroomInfo
                   {
                       Users = pc.Users.Select(u => u.Username).ToList(),
                       Owner = pc.Administrators.Owner.Username,
                       Moderators = pc.Administrators.Moderators.Select(u => u.Username).ToList(),
                       LastMessageTime = pc.LastMessageTime
                   };
    }
}