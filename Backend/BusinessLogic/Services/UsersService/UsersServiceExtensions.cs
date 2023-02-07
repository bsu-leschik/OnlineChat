using Constants;

namespace BusinessLogic.Services.UsersService;

public static class UsersServiceExtensions
{
    public static string? GetClaim(this IUserAccessor userAccessor, string claim)
    {
        if (!userAccessor.TryGetClaim(claim, out var value))
        {
            value = null;
        }
        return value;
    }

    public static string? GetUsername(this IUserAccessor userAccessor)
    {
        return userAccessor.GetClaim(Claims.Name);
    }

    public static Guid? GetToken(this IUserAccessor userAccessor)
    {
        if (Guid.TryParse(userAccessor.GetClaim(Claims.Token), out var token))
        {
            return token;
        }
        return null;
    }

    public static Guid? GetId(this IUserAccessor userAccessor)
    {
        if (userAccessor.TryGetClaim(Claims.UserId, out var id))
        {
            return Guid.Parse(id);
        }
        return null;
    }
}