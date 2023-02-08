using BusinessLogic.Services.UsersService;
using Constants;
using Database;
using Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, GetMessagesResponse>
{
    private readonly IUserAccessor _userAccessor;
    private readonly IStorageService _storageService;

    public GetMessagesHandler(IUserAccessor userAccessor, IStorageService storageService)
    {
        _userAccessor = userAccessor;
        _storageService = storageService;
    }

    private static GetMessagesResponse EmptyResponse => new(messages: null);

    public async Task<GetMessagesResponse> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userAccessor.GetId()!;
        var chatroom = await _storageService.GetChatroomTickets()
                                            .Where(ticket => ticket.UserId == userId && ticket.ChatroomId == request.ChatId)
                                            .Include(t => t.Chatroom)
                                            .ThenInclude(chatroom => chatroom.Messages
                                                                             .Skip(request.Offset)
                                                                             .Take(request.Count))
                                            .Select(t => t.Chatroom)
                                            .FirstOrDefaultAsync(cancellationToken);
        
        if (chatroom is null)
        {
            return EmptyResponse;
        }

        return new GetMessagesResponse(
            messages: chatroom.Messages.ToList()
        );
    }
}