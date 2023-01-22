using BusinessLogic.UsersService;
using Database;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Chatrooms.KickUserFromChatroom;

public class KickUserFromChatroomHandler : IRequestHandler<KickUserFromChatroomCommand, KickUserFromChatroomResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;
    private readonly IHttpContextAccessor _accessor;

    public KickUserFromChatroomHandler(IStorageService storageService, IUsersService usersService,
        IHttpContextAccessor accessor)
    {
        _storageService = storageService;
        _usersService = usersService;
        _accessor = accessor;
    }

    public async Task<KickUserFromChatroomResponse> Handle(KickUserFromChatroomCommand request,
        CancellationToken cancellationToken)
    {
        var chatroom = await _storageService.GetChatroomAsync(c => c.Id == request.ChatId, cancellationToken);
        if (chatroom is null || chatroom.Type == ChatType.Private)
        {
            return KickUserFromChatroomResponse.BadRequest;
        }
        
        var user = await _usersService.FindUser(_accessor.HttpContext!.User, cancellationToken);
        if (user is null)
        {
            return KickUserFromChatroomResponse.BadRequest;
        }
        
        if (!chatroom.Users.Contains(user))
        {
            return KickUserFromChatroomResponse.AccessDenied;
        }

        var toKick = chatroom.Users.Find(u => u.Username == request.Username);
        if (toKick is null)
        {
            return KickUserFromChatroomResponse.UserIsNotInChatroom;
        }

        var chat = (chatroom as PublicChatroom)!;
        if (toKick.IsOwnerOf(chat))
        {
            return KickUserFromChatroomResponse.AccessDenied;
        }
        if (user.IsOwnerOf(chat))
        {
            chat.Kick(toKick);
            await _storageService.SaveChangesAsync(cancellationToken);
            return KickUserFromChatroomResponse.Success;
        }
        if (user.IsModeratorOf(chat) && !toKick.IsModeratorOf(chat))
        {
            chat.Kick(toKick);
            await _storageService.SaveChangesAsync(cancellationToken);
            return KickUserFromChatroomResponse.Success;
        }
        return KickUserFromChatroomResponse.AccessDenied;
    }
}