using FunderMaps.Core.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    /// <summary>
    ///     Organization user repository.
    /// </summary>
    public interface IOrganizationUserRepository
    {
        ValueTask AddAsync(Guid organizationId, Guid userId, OrganizationRole role);

        ValueTask IsUserInOrganization(Guid organizationId, Guid userId);

        IAsyncEnumerable<Guid> ListAllAsync(Guid organizationId);
    }
}
