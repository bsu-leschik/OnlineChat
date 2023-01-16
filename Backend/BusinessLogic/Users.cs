using System.Security.Claims;
using Constants;
using Database;
using Database.Entities;

namespace BusinessLogic;

public static class Users
{
    public async static Task<User?> FindUser(IStorageService storageService, ClaimsPrincipal principal, CancellationToken cancellationToken)
    {
        var (username, token) = Decompose(principal);

        return await storageService.GetUserAsync(u => u.Username == username && u.Token == token, cancellationToken);
    }

    public static (string? Username, Guid? Token) Decompose(ClaimsPrincipal principal)
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