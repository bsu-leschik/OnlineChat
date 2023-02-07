using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services.UsersService;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _accessor;

    public UserAccessor(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    public bool TryGetClaim(string claimName, out string result)
    {
        var claims = _accessor.HttpContext!.User.Claims;
        var claim = claims.FirstOrDefault(c => c.Type == claimName);
        result = claim?.Value!;
        return claim is not null;
    }
}