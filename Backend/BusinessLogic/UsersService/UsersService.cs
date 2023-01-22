using System.Security.Claims;
using Constants;
using Database;
using Entities;

namespace BusinessLogic.UsersService;

public class UsersService : IUsersService
{
    private readonly IStorageService _storageService;

    public UsersService(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<User?> FindUser(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        var (username, token) = Decompose(claimsPrincipal);
        return await _storageService.GetUserAsync(u => u.Username == username && u.Token == token, cancellationToken);
    }

    public Task<(string? Username, Guid? Token)> Decompose(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
    {
        return Task.FromResult(Decompose(claimsPrincipal));
    }

    private static (string? Username, Guid? Token) Decompose(ClaimsPrincipal principal)
    {
        string? username = principal.FindFirstValue(Claims.Name);
        Guid? token = null;
        string? guid = principal.FindFirstValue(Claims.Token);
        if (Guid.TryParse(guid, out var id))
        {
            token = id;
        }
        return (username, token);
    }
}