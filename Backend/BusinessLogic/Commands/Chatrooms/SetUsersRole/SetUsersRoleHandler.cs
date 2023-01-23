using BusinessLogic.UsersService;
using Database;
using Entities.Chatrooms;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Chatrooms.SetUsersRole;

public class SetUsersRoleHandler : IRequestHandler<SetUsersRoleRequest, SetUsersRoleResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;
    private readonly IHttpContextAccessor _accessor;

    public SetUsersRoleHandler(IStorageService storageService, IUsersService usersService,
        IHttpContextAccessor accessor)
    {
        _storageService = storageService;
        _usersService = usersService;
        _accessor = accessor;
    }

    public async Task<SetUsersRoleResponse> Handle(SetUsersRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersService.FindUser(_accessor.HttpContext!.User, cancellationToken);

        var chatroom = user?.Chatrooms.FirstOrDefault(c => c.Id == request.ChatId);
        if (chatroom is null)
        {
            return SetUsersRoleResponse.BadRequest;
        }

        if (chatroom.Type == ChatType.Private)
        {
            return SetUsersRoleResponse.BadRequest;
        }

        var chat = (chatroom as PublicChatroom)!;
        if (chat.Administrators.Owner != user!)
        {
            return SetUsersRoleResponse.AccessDenied;
        }

        var toSetRole = chat.Users.FirstOrDefault(u => u.Username == request.Username);
        if (toSetRole is null)
        {
            return SetUsersRoleResponse.UserIsNotInTheChat;
        }

        if (request.NewRole != UsersRole.Moderator)
        {
            chat.RemoveModerator(toSetRole);
        }

        return SetUsersRoleResponse.Success;
    }
}