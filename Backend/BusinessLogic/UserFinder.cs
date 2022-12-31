using System.Security.Claims;
using BusinessLogic.Services;
using Database;
using Database.Entities;

namespace BusinessLogic;

public class UserFinder
{
    public static async Task<User?> FindUser(IStorageService storageService, ClaimsPrincipal principal, CancellationToken cancellationToken)
    {
        var claims = principal.Claims.ToList();
        var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var token = claims.FirstOrDefault(c => c.Type == "Token")?.Value;

        var user = storageService.GetUser(u => u.Username == username && u.Token.ToString() == token, cancellationToken);
        return await storageService.GetUser(u => u.Username == username && u.Token.ToString() == token, cancellationToken);
    }
}