using System.Security.Claims;
using Database;
using MediatR;

namespace BusinessLogic.Queries.Messages.GetMessages;

public class GetMessagesHandler : IRequestHandler<GetMessagesQuery, GetMessagesResponse>
{
    private readonly IStorageService _storageService;

    public GetMessagesHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<GetMessagesResponse> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var user = await UserFinder.FindUser(_storageService, ClaimsPrincipal.Current!, cancellationToken);
        return new GetMessagesResponse
                   {
                       Messages = user?.Chatrooms.FirstOrDefault(c => c.Id == request.ChatId)
                                      ?.Messages
                   };
    }
}