using System.Diagnostics;
using Entities;
using Entities.Chatrooms;

namespace BusinessLogic.Models;

public class ChatroomInfoBase
{
    public Guid Id { get; set; }
    public List<string> Users { get; set; }
    public ChatType Type { get; set; }
    public DateTime LastMessageTime { get; set; }
    public int UnreadMessages { get; set; }

    public ChatroomInfoBase(Guid id, List<string> users, ChatType type, DateTime lastMessageTime, int unreadMessages)
    {
        Id = id;
        Users = users;
        Type = type;
        LastMessageTime = lastMessageTime;
        UnreadMessages = unreadMessages;
    }
}

public static class ChatroomInfo
{
    public static object Of(ChatroomTicket ticket) => ticket.Chatroom switch
        {
            PublicChatroom pc => PublicChatroomInfo.Of(pc, ticket.LastMessageRead),
            PrivateChatroom pr => PrivateChatroomInfo.Of(pr, ticket.LastMessageRead),
            _ => throw new UnreachableException()
        };
}

public class PrivateChatroomInfo : ChatroomInfoBase
{
    public static PrivateChatroomInfo Of(PrivateChatroom pc, int messagesRead)
    {
        return new PrivateChatroomInfo(
            id: pc.Id,
            users: pc.Users.Select(u => u.Username).ToList(),
            lastMessageTime: pc.LastMessageTime,
            unreadMessages: pc.MessagesCount - messagesRead
        );
    }

    public PrivateChatroomInfo(Guid id, List<string> users, DateTime lastMessageTime, int unreadMessages)
        : base(id, users, ChatType.Private, lastMessageTime, unreadMessages) {}
}

public class PublicChatroomInfo : ChatroomInfoBase
{
    public string Owner { get; set; }
    public List<string> Moderators { get; set; }
    public string Name { get; set; }

    public static PublicChatroomInfo Of(PublicChatroom pc, int messagesRead)
    {
        return new PublicChatroomInfo(
            id: pc.Id,
            users: Enumerable.Empty<string>().ToList(),
            lastMessageTime: pc.LastMessageTime,
            unreadMessages: pc.MessagesCount - messagesRead,
            owner: pc.Administrators.Owner.Username,
            moderators: pc.Administrators.Moderators.Select(m => m.Username).ToList(),
            name: pc.Name);
    }

    public PublicChatroomInfo(Guid id, List<string> users, DateTime lastMessageTime, int unreadMessages, string owner, 
        List<string> moderators, string name)
        : base(id, users, ChatType.Public, lastMessageTime, unreadMessages)
    {
        Owner = owner;
        Moderators = moderators;
        Name = name;
    }
}