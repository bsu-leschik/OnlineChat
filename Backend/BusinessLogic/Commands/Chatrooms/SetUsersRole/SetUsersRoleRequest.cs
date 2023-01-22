using MediatR;

namespace BusinessLogic.Commands.Chatrooms.SetUsersRole;

public class SetUsersRoleRequest : IRequest<SetUsersRoleResponse>
{
    public string Username { get; set; }
    public UsersRole NewRole { get; set; }
    public Guid ChatId { get; set; }
}

public enum UsersRole
{
    Member = 0,
    Moderator
}