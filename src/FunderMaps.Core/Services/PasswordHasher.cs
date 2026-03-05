using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace FunderMaps.Core.Services;

/// <summary>
///     Password hasher.
/// </summary>
public class PasswordHasher(ILogger<PasswordHasher> logger)
{
    private const int IterRounds = 10_000;
    private const int SubkeyLength = 256 / 8; // 256 bits
    private const int SaltSize = 128 / 8; // 128 bits
    private const byte FormatMarker = 0x01;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    private static byte[] GeneratePasswordHash(string password)
    {
        byte[] salt = new byte[SaltSize];
        RandomNumberGenerator.Fill(salt);

        byte[] subkey = Rfc2898DeriveBytes.Pbkdf2(password, salt, IterRounds, HashAlgorithm, SubkeyLength);

        byte[] outputBytes = new byte[1 + SaltSize + SubkeyLength];
        outputBytes[0] = FormatMarker;
        Buffer.BlockCopy(salt, 0, outputBytes, 1, SaltSize);
        Buffer.BlockCopy(subkey, 0, outputBytes, 1 + SaltSize, SubkeyLength);
        return outputBytes;
    }

    private static bool VerifyHashedPassword(byte[] inputBytes, string password)
    {
        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(inputBytes, 1, salt, 0, SaltSize);

        byte[] expectedSubkey = new byte[SubkeyLength];
        Buffer.BlockCopy(inputBytes, 1 + SaltSize, expectedSubkey, 0, SubkeyLength);

        byte[] subkey = Rfc2898DeriveBytes.Pbkdf2(password, salt, IterRounds, HashAlgorithm, SubkeyLength);

        return CryptographicOperations.FixedTimeEquals(subkey, expectedSubkey);
    }

    /// <summary>
    ///     Hash plaintext password and return the password hash.
    /// </summary>
    public string HashPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password);
        return Convert.ToBase64String(GeneratePasswordHash(password));
    }

    /// <summary>
    ///     Check if password is valid.
    /// </summary>
    /// <remarks>
    ///     If anything fails in the process this method will return as if
    ///     the password validation failed. Exception details are logged.
    /// </remarks>
    public bool IsPasswordValid(string hashedPassword, string providedPassword)
    {
        ArgumentException.ThrowIfNullOrEmpty(hashedPassword);
        ArgumentException.ThrowIfNullOrEmpty(providedPassword);

        try
        {
            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            if (decodedHashedPassword[0] != FormatMarker)
            {
                return false;
            }

            return VerifyHashedPassword(decodedHashedPassword, providedPassword);
        }
        catch (SystemException exception)
        {
            logger.LogError(exception, "Error occurred during password validation");
            return false;
        }
    }
}
