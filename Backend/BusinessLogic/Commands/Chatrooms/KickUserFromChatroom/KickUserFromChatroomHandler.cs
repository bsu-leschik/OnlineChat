using BusinessLogic.Hubs.Chat;
using BusinessLogic.Services.UsersService;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace BusinessLogic.Commands.Chatrooms.KickUserFromChatroom;

public class KickUserFromChatroomHandler : IRequestHandler<KickUserFromChatroomCommand, KickUserFromChatroomResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;
    private readonly IHubContext<ChatHub> _hubContext;

    public KickUserFromChatroomHandler(IStorageService storageService, IUsersService usersService,
        IHttpContextAccessor accessor, IHubContext<ChatHub> hubContext)
    {
        _storageService = storageService;
        _usersService = usersService;
        _hubContext = hubContext;
    }

    public async Task<KickUserFromChatroomResponse> Handle(KickUserFromChatroomCommand request,
        CancellationToken cancellationToken)
    {
        var chatroom = await _storageService.GetChatroomById(request.ChatId, cancellationToken);
        if (chatroom is null || chatroom.Type == ChatType.Private)
        {
            return KickUserFromChatroomResponse.BadRequest;
        }
        
        var user = await _usersService.GetCurrentUser(cancellationToken);
        if (user is null)
        {
            return KickUserFromChatroomResponse.BadRequest;
        }
        
        if (!chatroom.Users.Contains(user))
        {
            return KickUserFromChatroomResponse.AccessDenied;
        }

        var toKick = chatroom.Users.FirstOrDefault(u => u.Username == request.Username);
        if (toKick is null)
        {
            return KickUserFromChatroomResponse.UserIsNotInChatroom;
        }

        var chat = (chatroom as PublicChatroom)!;
        if (toKick.IsOwnerOf(chat))
        {
            return KickUserFromChatroomResponse.AccessDenied;
        }
        if (user.IsOwnerOf(chat) || user.IsModeratorOf(chat) && !toKick.IsModeratorOf(chat))
        {
            return await Kick(chat, toKick, cancellationToken);
        }
        return KickUserFromChatroomResponse.AccessDenied;
    }

    private async Task<KickUserFromChatroomResponse> Kick(PublicChatroom chatroom, User user, CancellationToken cancellationToken)
    {
        chatroom.Kick(user);
        var saving = _storageService.SaveChangesAsync(cancellationToken);
        var notifying = _hubContext.NotifyUserKicked(chatroom.Id.ToString(), user.Username, cancellationToken);
        await Task.WhenAll(saving, notifying);
        return KickUserFromChatroomResponse.Success;
    }
}