using OnlineChat.Hubs.Reponse;
using OnlineChat.Services;

namespace OnlineChat.Hubs.ConnectionGuards;

public class ConnectionGuard
{
    private readonly List<IConnectionRequestApprover> _approvers = new();
    private readonly Storage _storage;
    private ConnectionResponseCode _responseCode = ConnectionResponseCode.SuccessfullyConnected;

    public ConnectionGuard(Storage storage)
    {
        _storage = storage;
    }

    public ConnectionGuard AddApprover(IConnectionRequestApprover approver)
    {
        _approvers.Add(approver);
        return this;
    }

    public bool Check(string nickname, int chatroomId)
    {
        _responseCode = ConnectionResponseCode.SuccessfullyConnected;
        var chatroom = _storage.GetChatroomById(chatroomId);
        
        if (chatroom is not null)
        {
            return _approvers.All(approver => 
                approver.Verify(chatroom, nickname, ref _responseCode));
        }
        
        _responseCode = ConnectionResponseCode.RoomDoesntExist;
        return false;
    }

    public ConnectionResponseCode GetResponseCode()
    {
        return _responseCode;
    }
}