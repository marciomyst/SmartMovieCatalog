using System.Security.Cryptography;
using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Infrastructure.Authentication;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    private const char Separator = '$';

    public string HashPassword(User user, string password)
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] key = Hash(password, salt, Iterations);

        return string.Join(
            Separator,
            "PBKDF2-SHA256",
            Iterations.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Convert.ToBase64String(salt),
            Convert.ToBase64String(key));
    }

    public bool VerifyPassword(User user, string password)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        string[] parts = user.PasswordHash.Split(Separator);
        if (parts.Length != 4 ||
            parts[0] != "PBKDF2-SHA256" ||
            !int.TryParse(parts[1], out int iterations))
        {
            return false;
        }

        byte[] salt;
        byte[] expectedKey;
        try
        {
            salt = Convert.FromBase64String(parts[2]);
            expectedKey = Convert.FromBase64String(parts[3]);
        }
        catch (FormatException)
        {
            return false;
        }

        byte[] actualKey = Hash(password, salt, iterations);
        return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
    }

    private static byte[] Hash(string password, byte[] salt, int iterations)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            KeySize);
    }
}
