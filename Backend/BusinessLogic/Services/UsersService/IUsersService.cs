using Entities;

namespace BusinessLogic.Services.UsersService;

public interface IUsersService
{
    public Task<User?> GetCurrentUser(CancellationToken cancellationToken);

    public Task<(string? Username, Guid? Token)> DecomposeCurrentPrincipal(CancellationToken cancellationToken);
    public bool TryGetClaim(string claimName, out string result);
}