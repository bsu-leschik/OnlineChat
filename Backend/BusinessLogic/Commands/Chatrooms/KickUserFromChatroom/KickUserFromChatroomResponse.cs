namespace BusinessLogic.Commands.Chatrooms.KickUserFromChatroom;

public enum KickUserFromChatroomResponse
{
    Success = 0,
    UserIsNotInChatroom,
    AccessDenied,
    BadRequest,
    Failed
}