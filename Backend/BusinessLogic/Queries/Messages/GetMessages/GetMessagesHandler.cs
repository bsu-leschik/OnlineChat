using BusinessLogic.UsersService;
using Database;
using MediatR;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, GetMessagesResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUsersService _usersService;

    public GetMessagesHandler(IStorageService storageService, IUsersService usersService)
    {
        _storageService = storageService;
        _usersService = usersService;
    }

    public async Task<GetMessagesResponse> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersService.GetCurrentUser(cancellationToken);
        return new GetMessagesResponse(
            messages: user?.Chatrooms.FirstOrDefault(c => c.Id == request.ChatId)
                ?.Messages
        );
    }
}