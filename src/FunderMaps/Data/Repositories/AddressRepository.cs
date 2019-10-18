using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Repositories;
using FunderMaps.Interfaces;
using FunderMaps.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Address repository.
    /// </summary>
    public class AddressRepository : RepositoryBase<Address, Guid>, IAddressRepository
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public AddressRepository(DbProvider dbProvider) : base(dbProvider) { }

        public override Task<Guid> AddAsync(Address entity)
        {
            throw new NotImplementedException();
        }

        public override Task<uint> CountAsync()
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Address entity)
        {
            throw new NotImplementedException();
        }

        public override Task<Address> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> GetOrAddAsync(Address address)
        {
            var sql = @"
                WITH insertornot AS (
                INSERT INTO application.address(
	                street_name, building_number, building_number_suffix)
	                VALUES (@StreetName, @BuildingNumber, @BuildingNumberSuffix)
	                ON CONFLICT DO NOTHING
	                RETURNING *
                )
                SELECT * FROM insertornot
                UNION
                SELECT *
			    FROM application.address
			    WHERE street_name=@StreetName
				    AND building_number=@BuildingNumber
				    AND ((@BuildingNumberSuffix IS NOT NULL AND building_number_suffix=@BuildingNumberSuffix)
				        OR
                        (@BuildingNumberSuffix IS NULL AND building_number_suffix IS NULL))";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Address>(sql, address));
            if (result.Count() == 0)
            {
                return null;
            }

            return result.First();
        }

        public override Task<IReadOnlyList<Address>> ListAllAsync(Navigation navigation)
        {
            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
