using OnlineChat.Hubs.Response;
using OnlineChat.Models;

namespace OnlineChat.Hubs.ConnectionGuards;

public class IsDuplicateNicknameApprover : IConnectionRequestApprover
{
    public bool Verify(Chatroom chatroom, string nickcname, ref ConnectionResponseCode code)
    {
        if (chatroom.Users.All(user => user.Nickname != nickcname))
        {
            return true;
        }

        code = ConnectionResponseCode.DuplicateNickname;
        return false;
    }
}

public class IsEmptyUsernameApprover : IConnectionRequestApprover
{
    public bool Verify(Chatroom chatroom, string nickcname, ref ConnectionResponseCode code)
    {
        if (nickcname != string.Empty)
        {
            return true;
        }

        code = ConnectionResponseCode.WrongNickname;
        return false;

    }
}