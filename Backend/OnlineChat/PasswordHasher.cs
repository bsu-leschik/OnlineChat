using Database.Entities;
using Microsoft.AspNetCore.Identity;

namespace OnlineChat;

/// <summary>
/// Temporary password hasher
/// </summary>
public class PasswordHasher : IPasswordHasher<User>
{
    public string HashPassword(User user, string password)
    {
        return password;
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        return hashedPassword == providedPassword
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}