using BusinessLogic.Models;
using BusinessLogic.Services.UsersService;
using MediatR;

namespace BusinessLogic.Queries.Chatrooms.GetChatrooms;

public class GetChatroomsHandler : IRequestHandler<GetChatroomsRequest, GetChatroomsResult>
{
    private readonly IUsersService _usersService;

    public GetChatroomsHandler(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<GetChatroomsResult> Handle(GetChatroomsRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersService.GetCurrentUser(cancellationToken);
        var result = user!.ChatroomTickets.Select(ChatroomInfo.Of).ToList();
        return new GetChatroomsResult(result);
    } 
}