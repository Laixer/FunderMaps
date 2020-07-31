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
        ValueTask<User> GetByEmailAsync(string email);

        ValueTask<uint> GetAccessFailedCountAsync(User entity);

        ValueTask<uint?> GetLoginCountAsync(User entity);

        ValueTask<DateTime?> GetLastLoginAsync(User entity);

        ValueTask<string> GetPasswordHashAsync(User entity);

        ValueTask SetPasswordHashAsync(User entity, string passwordHash);

        ValueTask BumpAccessFailed(User entity);

        ValueTask RegisterAccess(User entity);
    }
}
