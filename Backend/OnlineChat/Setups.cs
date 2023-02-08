using Constants;
using Extensions;
using Microsoft.AspNetCore.Authentication;

namespace OnlineChat;

public static class Setups
{
    public static IApplicationBuilder CheckClaimsIfNotAuthenticated(this IApplicationBuilder builder, params string[] claimNames)
    {
        builder.Use((context, next) =>
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                return next();
            }
            var claims = context.User.Claims.ToList();
            if (claims.ContainsClaims(claimNames))
            {
                return next();
            }
            context.SignOutAsync();
            context.Response.StatusCode = 400;
            return Task.CompletedTask;
        });
        return builder;
    }
}