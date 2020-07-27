using FunderMaps.Core.Entities;
using System;
using System.Threading.Tasks;

namespace FunderMaps.Core.Interfaces.Repositories
{
    public interface IOrganizationRepository : IAsyncRepository<Organization, Guid>
    {
        ValueTask<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash);

        ValueTask<Organization> GetByNameAsync(string name);

        ValueTask<Organization> GetByEmailAsync(string email);
    }
}
