using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Extensions;
using FunderMaps.Interfaces;

namespace FunderMaps.Data.Repositories
{
    public class OrganizationRepository : EfRepository<FisDbContext, Organization>, IOrganizationRepository
    {
        public OrganizationRepository(FisDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<Organization> GetOrAddAsync(Organization organization)
            => _dbContext.Organization.GetOrAddAsync(organization, s => s.Id == organization.Id ||
                    s.Name == organization.Name);
    }
}
