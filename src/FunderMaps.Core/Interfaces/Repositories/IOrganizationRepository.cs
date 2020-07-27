using FunderMaps.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
    {
        ValueTask<Organization> GetByNameAsync(string name);

        ValueTask<Organization> GetByEmailAsync(string email);
    }
}
