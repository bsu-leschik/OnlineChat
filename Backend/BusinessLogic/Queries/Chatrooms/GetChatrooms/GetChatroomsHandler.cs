using BusinessLogic.Models;
using BusinessLogic.Services.UsersService;
using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsHandler : IRequestHandler<GetChatroomsRequest, GetChatroomsResult>
{
    private readonly IUsersService _usersService;
    private readonly IStorageService _storageService;

    public GetChatroomsHandler(IUsersService usersService, IStorageService storageService)
    {
        _usersService = usersService;
        _storageService = storageService;
    }

    public async Task<GetChatroomsResult> Handle(GetChatroomsRequest request, CancellationToken cancellationToken)
    {
        var id = _usersService.GetId()!;
        var tickets = await _storageService.GetUsersChatroomTickets(id.Value)
                                     .ToListAsync(cancellationToken);
        var result = tickets.Select(ChatroomInfo.Of).ToList();
        result.Sort(SortComparator);
        return new GetChatroomsResult(result);
    }

    private static int SortComparator(object a, object b)
    {
        return -(a as ChatroomInfoBase)!.LastMessageTime.CompareTo((b as ChatroomInfoBase)!.LastMessageTime);
    }
}