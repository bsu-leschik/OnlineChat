using Database;
using Database.Entities;
using Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsHandler : IRequestHandler<GetChatroomsRequest, List<ChatroomInfo>>
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _accessor;

    public GetChatroomsHandler(IStorageService storageService, IHttpContextAccessor accessor)
    {
        _storageService = storageService;
        _accessor = accessor;
    }

    public async Task<List<ChatroomInfo>> Handle(GetChatroomsRequest request, CancellationToken cancellationToken)
    {
        bool IsInChat(Chatroom chatroom, User user)
        {
            return chatroom.Users.Contains(u => u.Username == user.Username);
        }

        var user = await BusinessLogic.Users.FindUser(_storageService, _accessor.HttpContext!.User, cancellationToken);
        if (user == null)
        {
            return new List<ChatroomInfo>();
        }

        return user.Chatrooms
                   .Select(ChatroomInfo.Of)
                   .ToList();
    }
}