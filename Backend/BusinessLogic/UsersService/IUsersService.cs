using System.Security.Claims;
using Database.Entities;

namespace BusinessLogic.UsersService;

public interface IUsersService
{
    public Task<User?> FindUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);

    public Task<(string? Username, Guid? Token)> Decompose(ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken);
}