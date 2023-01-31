using BusinessLogic.Models;
using BusinessLogic.Services.UsersService;
using MediatR;

namespace BusinessLogic.Queries.Chatrooms.GetChatroomInfo;

public class GetChatroomInfoHandler : IRequestHandler<GetChatroomInfoRequest, object?>
{
    private readonly IUsersService _usersService;

    public GetChatroomInfoHandler(IUsersService usersService)
    {
        _usersService = usersService;
    }

    public async Task<object?> Handle(GetChatroomInfoRequest request, CancellationToken cancellationToken)
    {
        var user = await _usersService.GetCurrentUser(cancellationToken);
        var chatroomTicket = user!.ChatroomTickets.FirstOrDefault(t => t.Chatroom.Id == request.ChatId);
        if (chatroomTicket is null)
        {
            return null;
        }

        return ChatroomInfo.Of(chatroomTicket);
    }
}