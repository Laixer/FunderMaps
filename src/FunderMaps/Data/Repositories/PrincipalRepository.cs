﻿using System.Threading.Tasks;
using FunderMaps.Core.Entities;
using FunderMaps.Extensions;
using FunderMaps.Interfaces;

namespace FunderMaps.Data.Repositories
{
    public class PrincipalRepository //: RepositoryBase<FisDbContext, Principal>, IPrincipalRepository
    {
        public PrincipalRepository()
            //: base(dbContext)
        {
        }

        public Task<Principal> GetOrAddAsync(Principal principal)
        {
            //_dbContext.Principal.GetOrAddAsync(principal, s => s.Id == principal.Id ||
            //        s.NickName == principal.NickName ||
            //        s.Email == principal.Email);
            return null;
        }
    }
}
