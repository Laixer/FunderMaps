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
        /// <param name="entity">User object.</param>
        /// <returns>Number of failed signins.</returns>
        ValueTask<uint> GetAccessFailedCountAsync(User entity);

        /// <summary>
        ///     Get signin count.
        /// </summary>
        /// <param name="entity">User object.</param>
        /// <returns>Number of signins.</returns>
        ValueTask<uint?> GetLoginCountAsync(User entity);

        /// <summary>
        ///     Get last sign in.
        /// </summary>
        /// <param name="entity">User object.</param>
        /// <returns>Datetime of last signin.</returns>
        ValueTask<DateTime?> GetLastLoginAsync(User entity);

        /// <summary>
        ///     Get password hash.
        /// </summary>
        /// <param name="entity">User object.</param>
        /// <returns>Password hash as string.</returns>
        ValueTask<string> GetPasswordHashAsync(User entity);

        /// <summary>
        ///     Whether or not user is locked out.
        /// </summary>
        /// <param name="entity">User object.</param>
        /// <returns>True if locked out, false otherwise.</returns>
        ValueTask<bool> IsLockedOutAsync(User entity);

        /// <summary>
        ///     Update user password.
        /// </summary>
        /// <param name="entity">User object.</param>
        /// <param name="passwordHash">New password hash.</param>
        ValueTask SetPasswordHashAsync(User entity, string passwordHash);

        /// <summary>
        ///     Increase signin failure count.
        /// </summary>
        /// <param name="entity">User object.</param>
        ValueTask BumpAccessFailed(User entity);

        /// <summary>
        ///     Reset signin failure count.
        /// </summary>
        /// <param name="entity">User object.</param>
        ValueTask ResetAccessFailed(User entity);

        /// <summary>
        ///     Register a new user signin.
        /// </summary>
        /// <param name="entity">User object.</param>
        ValueTask RegisterAccess(User entity);
    }
}
