using BusinessLogic.UsersService;
using Database;
using Entities;
using Entities.Chatrooms;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsHandler : IRequestHandler<GetChatroomsRequest, List<ChatroomInfo>>
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _accessor;
    private readonly IUsersService _usersService;

    public GetChatroomsHandler(IStorageService storageService, IHttpContextAccessor accessor, IUsersService usersService)
    {
        _storageService = storageService;
        _accessor = accessor;
        _usersService = usersService;
    }

    public async Task<List<ChatroomInfo>> Handle(GetChatroomsRequest request, CancellationToken cancellationToken)
    {
        bool IsInChat(Chatroom chatroom, User user)
        {
            return chatroom.Users.Contains(u => u.Username == user.Username);
        }

        var user = await _usersService.FindUser(_accessor.HttpContext!.User, cancellationToken);
        if (user == null)
        {
            return new List<ChatroomInfo>();
        }

        return await _storageService.GetChatroomsAsync(cancellationToken)
                              .WhereAsync(chat => IsInChat(chat, user), cancellationToken)
                              .SelectAsync(ChatroomInfo.Of, cancellationToken)
                              .ToListAsync(cancellationToken);
        return user.Chatrooms
                   .Select(ChatroomInfo.Of)
                   .ToList();
    }
}