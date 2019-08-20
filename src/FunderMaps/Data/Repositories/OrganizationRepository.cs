using FunderMaps.Core.Entities.Fis;
using FunderMaps.Extensions;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    public class OrganizationRepository // : RepositoryBase<Organization, int>, IOrganizationRepository
    {
        public OrganizationRepository(DbProvider dbProvider)
            //: base(dbProvider)
        {
        }

        public Task<Organization> GetOrAddAsync(Organization organization)
        {
            //_dbContext.Organization.GetOrAddAsync(organization, s => s.Id == organization.Id ||
            //        s.Name == organization.Name);

            return null;
        }
    }
}
