using Constants;

namespace BusinessLogic.Services.UsersService;

public static class UsersServiceExtensions
{
    public static string? GetClaim(this IUsersService usersService, string claim)
    {
        if (!usersService.TryGetClaim(claim, out var value))
        {
            value = null;
        }
        return value;
    }

    public static string? GetUsername(this IUsersService usersService)
    {
        return usersService.GetClaim(Claims.Name);
    }

    public static Guid? GetToken(this IUsersService usersService)
    {
        if (Guid.TryParse(usersService.GetClaim(Claims.Token), out var token))
        {
            return token;
        }
        return null;
    }

    public static Guid? GetId(this IUsersService usersService)
    {
        if (usersService.TryGetClaim(Claims.UserId, out var id))
        {
            return Guid.Parse(id);
        }
        return null;
    }
}