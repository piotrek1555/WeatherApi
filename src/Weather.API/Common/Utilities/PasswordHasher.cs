using Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Common.Utilities;

public static class PasswordHasher
{
    public static string HashPassword(string password)
    {
        return new PasswordHasher<User>().HashPassword(new User(), password);
    }

    public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var result = new PasswordHasher<User>().VerifyHashedPassword(new User(), hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}