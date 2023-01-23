using BusinessLogic.UsersService;
using Database;
using Entities.Chatrooms;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Chatrooms.LeaveChatroom;

public class LeaveChatroomHandler : IRequestHandler<LeaveChatroomCommand, LeaveChatroomResponse>
{
    private readonly IUsersService _usersService;
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _accessor;

    public LeaveChatroomHandler(IUsersService usersService, IStorageService storageService,
        IHttpContextAccessor accessor)
    {
        _usersService = usersService;
        _storageService = storageService;
        _accessor = accessor;
    }

    public async Task<LeaveChatroomResponse> Handle(LeaveChatroomCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersService.FindUser(_accessor.HttpContext!.User, cancellationToken);
        if (user is null)
        {
            return LeaveChatroomResponse.BadRequest;
        }

        var chatroom = user.Chatrooms.FirstOrDefault(c => c.Id == request.ChatId);
        if (chatroom is null)
        {
            return LeaveChatroomResponse.NotInChatroom;
        }

        if (chatroom.Type == ChatType.Private)
        {
            return LeaveChatroomResponse.BadRequest;
        }

        var chat = (chatroom as PublicChatroom)!;
        if (chat.Kick(user))
        {
            await _storageService.SaveChangesAsync(cancellationToken);
            return LeaveChatroomResponse.Success;
        }

        if (chat.Administrators.Owner != user)
        {
            return LeaveChatroomResponse.BadRequest;
        }

        if (chat.Users.Count == 1)
        {
            user.Leave(chat);
            await _storageService.RemoveAsync(chat, cancellationToken);
            await _storageService.SaveChangesAsync(cancellationToken);
            return LeaveChatroomResponse.Success;
        }

        var newOwner = chat.Administrators.Moderators.FirstOrDefault() ?? chat.Users.First();

        chat.ForceKick(user);
        chat.Administrators.Owner = newOwner;
        await _storageService.SaveChangesAsync(cancellationToken);
        return LeaveChatroomResponse.Success;
    }
}