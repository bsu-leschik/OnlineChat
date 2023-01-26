using System.Security.Claims;
using Entities;

namespace BusinessLogic.UsersService;

public interface IUsersService
{
    public Task<User?> GetCurrentUser(CancellationToken cancellationToken);

    public Task<(string? Username, Guid? Token)> DecomposeCurrentPrincipal(CancellationToken cancellationToken);
}