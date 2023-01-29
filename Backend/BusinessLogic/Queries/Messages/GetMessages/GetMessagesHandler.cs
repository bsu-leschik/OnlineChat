using BusinessLogic.Services.UsersService;
using Constants;
using Database;
using Extensions;
using MediatR;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, GetMessagesResponse>
{
    private readonly IUsersService _usersService;
    private readonly IStorageService _storageService;

    public GetMessagesHandler(IUsersService usersService, IStorageService storageService)
    {
        _usersService = usersService;
        _storageService = storageService;
    }

    private static GetMessagesResponse EmptyResponse => new(messages: null);

    public async Task<GetMessagesResponse> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var chatroom = await _storageService.GetChatroomAsync(c => c.Id == request.ChatId, cancellationToken);
        if (chatroom is null)
        {
            return EmptyResponse;
        }

        var username = _usersService.GetUsername()!;

        if (!chatroom.Users.Contains(u => u.Username == username))
        {
            return EmptyResponse;
        }

        return new GetMessagesResponse(
            messages: chatroom.Messages
        );
    }
}