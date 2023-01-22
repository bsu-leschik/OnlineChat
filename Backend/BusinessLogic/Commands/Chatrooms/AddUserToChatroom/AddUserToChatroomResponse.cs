namespace BusinessLogic.Commands.Chatrooms.AddUserToChatroom;

public enum AddUserToChatroomResponse
{
    Success = 0,
    UserIsAlreadyInTheChat,
    ChatIsPrivate,
    UserDoesntExist,
    ChatroomDoesntExist,
    AccessDenied
}