namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Provides an abstraction for hashing passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    ///     Returns a hashed representation of the supplied <paramref name="password"/>.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed representation of the supplied <paramref name="password"/>.</returns>
    string HashPassword(string password);

    /// <summary>
    ///     Returns a boolean indicating the result of a password hash comparison.
    /// </summary>
    /// <param name="hashedPassword">The hashed password.</param>
    /// <param name="providedPassword">The password supplied for comparison.</param>
    /// <returns>True indicating the password is valid.</returns>
    /// <remarks>Implementations of this method should be time consistent.</remarks>
    bool IsPasswordValid(string hashedPassword, string providedPassword);
}
