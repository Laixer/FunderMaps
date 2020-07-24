using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IUserRepository : IAsyncRepository<User, Guid>
    {
        ValueTask<User> GetByIdAndPasswordHashAsync(Guid id, string passwordHash);

        ValueTask<User> GetByEmailAsync(string email);

        ValueTask<User> GetByEmailAndPasswordHashAsync(string email, string passwordHash);

        ValueTask SetPasswordHashAsync(User entity, string passwordHash);

        ValueTask BumpAccessFailed(User entity);

        ValueTask RegisterAccess(User entity);
    }
}
