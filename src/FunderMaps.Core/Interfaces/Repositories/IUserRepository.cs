using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
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
        ValueTask<User> GetByEmailAsync(string email);

        /// <summary>
        ///     Get signin faillure count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>Number of failed signins.</returns>
        ValueTask<uint> GetAccessFailedCountAsync(Guid id);

        /// <summary>
        ///     Get signin count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>Number of signins.</returns>
        ValueTask<uint> GetLoginCountAsync(Guid id);

        /// <summary>
        ///     Get last sign in.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>Datetime of last signin.</returns>
        ValueTask<DateTime?> GetLastLoginAsync(Guid id);

        /// <summary>
        ///     Get password hash.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>Password hash as string.</returns>
        ValueTask<string> GetPasswordHashAsync(Guid id);

        /// <summary>
        ///     Whether or not user is locked out.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <returns>True if locked out, false otherwise.</returns>
        ValueTask<bool> IsLockedOutAsync(Guid id);

        /// <summary>
        ///     Update user password.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        /// <param name="passwordHash">New password hash.</param>
        ValueTask SetPasswordHashAsync(Guid id, string passwordHash);

        /// <summary>
        ///     Increase signin failure count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        ValueTask BumpAccessFailed(Guid id);

        /// <summary>
        ///     Reset signin failure count.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        ValueTask ResetAccessFailed(Guid id);

        /// <summary>
        ///     Register a new user login.
        /// </summary>
        /// <param name="id">Entity identifier.</param>
        ValueTask RegisterAccess(Guid id);
    }
}
