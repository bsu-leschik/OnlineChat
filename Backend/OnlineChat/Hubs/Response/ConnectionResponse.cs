using OnlineChat.Models;

namespace OnlineChat.Hubs.Response;

public class ConnectionResponse
{
    public List<Message>? Messages { get; set; }
    public ConnectionResponseCode Response { get; set; }
}

public enum ConnectionResponseCode
{
    SuccessfullyConnected = 0,
    AccessDenied,
    DuplicateNickname,
    BannedNickname,
    RoomIsFull,
    WrongNickname,
    RoomDoesntExist
}