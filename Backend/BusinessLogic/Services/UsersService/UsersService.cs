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
        var id = this.GetId();
        if (id is null)
        {
            return null;
        }

        var user = await _storageService.GetUserById(id.Value, cancellationToken);
        if (user is null)
        {
            return null;
        }

        return user.Token == this.GetToken()
            ? (_user = user)
            : null;
    }

    public bool TryGetClaim(string claimName, out string result)
    {
        var claims = _accessor.HttpContext!.User.Claims;
        var claim = claims.FirstOrDefault(c => c.Type == claimName);
        result = claim?.Value!;
        return claim is not null;
    }
}