using System.Security.Claims;
using Constants;
using Database;
using Entities;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services.UsersService;

public class UsersService : IUsersService
{
    private readonly IStorageService _storageService;
    private readonly IHttpContextAccessor _accessor;
    private User? _user;

    public UsersService(IStorageService storageService, IHttpContextAccessor accessor)
    {
        _storageService = storageService;
        _accessor = accessor;
    }

    public async Task<User?> GetCurrentUser(CancellationToken cancellationToken)
    {
        if (_user is not null)
        {
            return _user;
        }
        var (username, token) = Decompose(_accessor.HttpContext!.User);
        if (username is null || token is null)
        {
            return null;
        }
        var user = await _storageService.GetUserAsync(u => u.Username == username, cancellationToken);
        _user = user?.Token == token
            ? user
            : null;
        return _user;
    }

    public Task<(string? Username, Guid? Token)> DecomposeCurrentPrincipal(CancellationToken cancellationToken)
    {
        return Task.FromResult(Decompose(_accessor.HttpContext!.User));
    }

    public bool TryGetClaim(string claimName, out string result)
    {
        var claims = _accessor.HttpContext!.User.Claims;
        var claim = claims.FirstOrDefault(c => c.Type == claimName);
        result = claim?.Value!;
        return claim is not null;
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