namespace BusinessLogic.Commands.Chatrooms.SetUsersRole;

public enum SetUsersRoleResponse
{
    Success = 0,
    AccessDenied,
    UserIsNotInTheChat,
    BadRequest
}