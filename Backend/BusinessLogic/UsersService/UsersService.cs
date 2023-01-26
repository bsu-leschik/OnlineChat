using System.Security.Claims;
using Constants;
using Database;
using Entities;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.UsersService;

public class UsersService : IUsersService
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _accessor;

    public UsersService(IStorageService storageService, IHttpContextAccessor accessor)
    {
        _storageService = storageService;
        _accessor = accessor;
    }

    public async Task<User?> GetCurrentUser(CancellationToken cancellationToken)
    {
        var (username, token) = Decompose(_accessor.HttpContext!.User);
        if (username is null || token is null)
        {
            return null;
        }
        var user = await _storageService.GetUserAsync(u => u.Username == username, cancellationToken);
        return user?.Token == token
            ? user
            : null;
    }

    public Task<(string? Username, Guid? Token)> DecomposeCurrentPrincipal(CancellationToken cancellationToken)
    {
        return Task.FromResult(Decompose(_accessor.HttpContext!.User));
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