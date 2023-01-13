using Database;
using Extensions;
using MediatR;

namespace BusinessLogic.Queries.Users.GetUsernames;

public class GetUsernamesHandler : IRequestHandler<GetUsernamesQuery, GetUsernamesResponse>
{
    private readonly IStorageService _storageService;

    public GetUsernamesHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<GetUsernamesResponse> Handle(GetUsernamesQuery request, CancellationToken cancellationToken)
    {
        if (request.StartingWith is null)
        {
            return new GetUsernamesResponse(
                await _storageService
                      .GetUsersAsync(cancellationToken)
                      .SelectAsync(u => u.Username, cancellationToken)
                      .ToListAsync(cancellationToken)
            );
        }

        return new GetUsernamesResponse(
            await _storageService
                  .GetUsersAsync(cancellationToken)
                  .WhereAsync(u => u.Username.StartsWith(request.StartingWith), cancellationToken)
                  .SelectAsync(u => u.Username, cancellationToken)
                  .ToListAsync(cancellationToken)
        );
    }
}