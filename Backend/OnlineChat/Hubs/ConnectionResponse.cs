using Database.Entities;

namespace OnlineChat.Hubs;

public class ConnectionResponse
{
    public List<Message>? Messages { get; set; }
    public ConnectionResponseCode Response { get; set; }

    public ConnectionResponse(List<Message>? messages, ConnectionResponseCode response)
    {
        Messages = messages;
        Response = response;
    }
}

public enum ConnectionResponseCode
{
    SuccessfullyConnected = 0,
    AccessDenied,
    Error,
    DuplicateNickname,
    BannedNickname,
    RoomIsFull,
    WrongNickname,
    RoomDoesntExist
}