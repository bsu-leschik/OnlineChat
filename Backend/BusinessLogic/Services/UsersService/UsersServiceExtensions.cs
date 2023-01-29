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

    public static string? GetToken(this IUsersService usersService)
    {
        return usersService.GetClaim(Claims.Token);
    }
}