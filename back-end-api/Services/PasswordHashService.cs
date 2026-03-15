using Microsoft.AspNetCore.Identity;

namespace BackEndApi.Services;

public static class PasswordHash
{
    private static readonly PasswordHasher<object> Hasher = new();

    public static string Hash(string password)
    {
        return Hasher.HashPassword(null, password);
    }

    public static bool Verify(string provided, string hashed)
    {
        var result = Hasher.VerifyHashedPassword(null, hashed, provided);
        return result == PasswordVerificationResult.Success;
    }
}