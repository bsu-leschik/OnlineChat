using Entities;
using Entities.Chatrooms;

namespace Extensions;

public static class UserExtensions
{
    public static bool IsOwnerOf(this User user, PublicChatroom chatroom)
    {
        return chatroom.Owner == user;
    }

    public static bool IsModeratorOf(this User user, PublicChatroom chatroom)
    {
        return chatroom.Moderators.Contains(user);
    }

    public static bool IsMemberOf(this User user, Chatroom chatroom)
    {
        return chatroom.Users.Contains(user);
    }
}