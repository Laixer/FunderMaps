using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunderMaps.Core.Entities.Fis;
using FunderMaps.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FunderMaps.Data.Repositories
{
    public class AddressRepository : EfRepository<FisDbContext, Address>, IAddressRepository
    {
        public AddressRepository(FisDbContext dbContext)
            : base(dbContext)
        {
        }

        public Task<Address> GetByIdAsync(Guid id)
        {
            return _dbContext.Address.FindAsync(id);
        }

        public Task<Address> GetByAddressAsync(string street, short building_number, string building_number_suffix)
        {
            return _dbContext.Address.FirstOrDefaultAsync(s => s.StreetName == street &&
                s.BuildingNumber == building_number &&
                s.BuildingNumberSuffix == building_number_suffix);
        }
    }
}
