using BusinessLogic.Models;
using BusinessLogic.Services.UsersService;
using Database;
using Extensions;
using MediatR;

namespace BusinessLogic.Queries.Chatrooms.GetChatroomInfo;

public class GetChatroomInfoHandler : IRequestHandler<GetChatroomInfoRequest, object?>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public GetChatroomInfoHandler(IStorageService storageService, IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
    }

    public async Task<object?> Handle(GetChatroomInfoRequest request, CancellationToken cancellationToken)
    {
        var chatroom = await _storageService.GetChatroomAsync(c => c.Id == request.ChatId, cancellationToken);
        if (chatroom is null)
        {
            return null;
        }
        var username = _usersService.GetUsername()!;
        return chatroom.Users.Contains(u => u.Username == username)
            ? ChatroomInfo.Of(chatroom)
            : null;
    }
}