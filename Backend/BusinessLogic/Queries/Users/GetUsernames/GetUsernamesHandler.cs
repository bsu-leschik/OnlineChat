using Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                await _storageService.GetUsers()
                                     .Select(u => u.Username)
                                     .Take(request.Limit)
                                     .ToListAsync(cancellationToken)
            );
        }

        return new GetUsernamesResponse(
            await _storageService
                .GetUsers()
                .Where(u => u.Username.StartsWith(request.StartingWith))
                .Select(u => u.Username)
                .Take(request.Limit)
                .ToListAsync(cancellationToken)
        );
    }
}