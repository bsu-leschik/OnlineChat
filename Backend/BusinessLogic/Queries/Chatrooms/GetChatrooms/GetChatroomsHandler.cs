using BusinessLogic.Models;
using BusinessLogic.Services.UsersService;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsHandler : IRequestHandler<GetChatroomsRequest, GetChatroomsResult>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public GetChatroomsHandler(IStorageService storageService, IHttpContextAccessor accessor, IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
    }

    public async Task<GetChatroomsResult> Handle(GetChatroomsRequest request, CancellationToken cancellationToken)
    {
        var username = _usersService.GetUsername()!;
        
        bool IsInChat(Chatroom chatroom)
        {
            return chatroom.Users.Contains(u => u.Username == username);
        }
        
        return new GetChatroomsResult(await _storageService.GetChatroomsAsync(cancellationToken)
                              .WhereAsync(IsInChat, cancellationToken)
                              .SelectAsync(ChatroomInfo.Of, cancellationToken)
                              .ToListAsync(cancellationToken));
    }
}