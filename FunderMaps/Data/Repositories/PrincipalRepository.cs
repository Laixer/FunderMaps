using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Extensions;
using FunderMaps.Interfaces;

namespace FunderMaps.Data.Repositories
{
    public class PrincipalRepository : EfRepository<FisDbContext, Principal>, IPrincipalRepository
    {
        public PrincipalRepository(FisDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<Principal> GetOrAddAsync(Principal principal)
            => _dbContext.Principal.GetOrAddAsync(principal, s => s.Id == principal.Id ||
                    s.NickName == principal.NickName ||
                    s.Email == principal.Email);
    }
}
