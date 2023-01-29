using System.Diagnostics;
using BusinessLogic.Models;
using BusinessLogic.Services.UsersService;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsHandler : IRequestHandler<GetChatroomsRequest, List<object>>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public GetChatroomsHandler(IStorageService storageService, IHttpContextAccessor accessor, IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
    }

    public async Task<List<object>> Handle(GetChatroomsRequest request, CancellationToken cancellationToken)
    {
        bool IsInChat(Chatroom chatroom, User user)
        {
            return chatroom.Users.Contains(u => u.Username == user.Username);
        }

        var user = await _usersService.GetCurrentUser(cancellationToken);
        if (user is null)
        {
            return new List<object>();
        }

        return await _storageService.GetChatroomsAsync(cancellationToken)
                              .WhereAsync(chat => IsInChat(chat, user), cancellationToken)
                              .SelectAsync(ChatroomInfo.Of, cancellationToken)
                              .ToListAsync(cancellationToken);
    }
}