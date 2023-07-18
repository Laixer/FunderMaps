using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces.Repositories;

/// <summary>
///     User repository.
/// </summary>
public interface IUserRepository : IAsyncRepository<User, Guid>
{
    /// <summary>
    ///     Retrieve <see cref="User"/> by email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <returns><see cref="User"/>.</returns>
    Task<User> GetByEmailAsync(string email);

    /// <summary>
    ///     Retrieve <see cref="User"/> by authentication key.
    /// </summary>
    /// <param name="key">Authentication key.</param>
    /// <returns><see cref="User"/>.</returns>
    Task<User> GetByAuthKeyAsync(string key);

    /// <summary>
    ///     Get password hash.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Password hash as string.</returns>
    Task<string> GetPasswordHashAsync(Guid id);

    /// <summary>
    ///     Get access failed count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <returns>Failed access count.</returns>
    Task<int> GetAccessFailedCount(Guid id);

    /// <summary>
    ///     Update user password.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    /// <param name="passwordHash">New password hash.</param>
    Task SetPasswordHashAsync(Guid id, string passwordHash);

    /// <summary>
    ///     Increase signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    Task BumpAccessFailed(Guid id);

    /// <summary>
    ///     Reset signin failure count.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    Task ResetAccessFailed(Guid id);

    /// <summary>
    ///     Register a new user login.
    /// </summary>
    /// <param name="id">Entity identifier.</param>
    Task RegisterAccess(Guid id);
}
