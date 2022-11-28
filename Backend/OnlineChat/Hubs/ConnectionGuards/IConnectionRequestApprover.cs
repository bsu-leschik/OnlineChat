using OnlineChat.Hubs.Response;
using OnlineChat.Models;

namespace OnlineChat.Hubs.ConnectionGuards;

public interface IConnectionRequestApprover
{
    public bool Verify(Chatroom chatroom, string nickcname, ref ConnectionResponseCode code);
}