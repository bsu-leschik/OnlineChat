using BusinessLogic.UsersService;
using Database;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Commands.Chatrooms.AddUserToChatroom;

public class AddUserToChatroomHandler : IRequestHandler<AddUserToChatroomCommand, AddUserToChatroomResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public AddUserToChatroomHandler(IStorageService storageService, IUsersService usersService, IHttpContextAccessor accessor)
    {
        _storageService = storageService;
        _usersService = usersService;
    }

    public async Task<AddUserToChatroomResponse> Handle(AddUserToChatroomCommand request, CancellationToken cancellationToken)
    {
        var (currentUsername, currentToken) = await _usersService.DecomposeCurrentPrincipal(cancellationToken);
        if (currentUsername is null || currentToken is null)
        {
            return AddUserToChatroomResponse.AccessDenied;
        }
        
        var chatroom = await _storageService.GetChatroomAsync(c => c.Id == request.ChatId, cancellationToken);
        if (chatroom is null)
        {
            return AddUserToChatroomResponse.ChatroomDoesntExist;
        }

        if (chatroom.Type == ChatType.Private)
        {
            return AddUserToChatroomResponse.ChatIsPrivate;
        }
        
        if (!chatroom.Users.Contains(u => u.Username == currentUsername && u.Token == currentToken))
        {
            return AddUserToChatroomResponse.AccessDenied;
        }

        if (chatroom.Users.Contains(u => u.Username == request.Username))
        {
            return AddUserToChatroomResponse.UserIsAlreadyInTheChat;
        }

        var user = await _storageService.GetUserAsync(u => u.Username == request.Username, cancellationToken);
        if (user is null)
        {
            return AddUserToChatroomResponse.UserDoesntExist;
        }
        
        chatroom.Users.Add(user);
        await _storageService.SaveChangesAsync(cancellationToken);
        return AddUserToChatroomResponse.Success;
    }
}