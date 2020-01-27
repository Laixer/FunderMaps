using Dapper;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Repositories;
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
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            throw new NotImplementedException();
        }

        public override Task<uint> CountAsync()
        {
            throw new NotImplementedException();
        }

        public override Task DeleteAsync(Address entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            throw new NotImplementedException();
        }

        public override Task<Address> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Address2>> GetByStreetNameAsync(string streetName, uint limit = 100)
        {
            if (string.IsNullOrEmpty(streetName))
            {
                throw new ArgumentNullException(nameof(streetName));
            }

            var sql = @"
                SELECT street, city
                FROM geospatial.address
                WHERE street LIKE LOWER(@Query)
                GROUP BY city, street
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Address2>(sql, new
            {
                Query = streetName + "%",
                Limit = (int)limit,
            }));
            if (!result.Any())
            {
                return null;
            }

            return result.ToArray();
        }

        public async Task<Address> GetOrAddAsync(Address entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                WITH insertornot AS (
                    INSERT INTO application.address(street_name, building_number, building_number_suffix, bag)
	                VALUES (@StreetName, @BuildingNumber, @BuildingNumberSuffix, @Bag)
	                ON CONFLICT DO NOTHING
	                RETURNING *
                )
                SELECT * FROM insertornot
                UNION
                SELECT *
			    FROM application.address
			    WHERE (street_name=@StreetName
				    AND building_number=@BuildingNumber
				    AND ((@BuildingNumberSuffix IS NOT NULL AND building_number_suffix=@BuildingNumberSuffix)
				        OR
                        (@BuildingNumberSuffix IS NULL AND building_number_suffix IS NULL)))
                    OR bag=@Bag";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Address>(sql, entity));
            if (!result.Any())
            {
                return null;
            }

            var address = result.First();

            if (!string.IsNullOrEmpty(entity.StreetName) && !string.IsNullOrEmpty(entity.Bag))
            {
                await RunSqlCommand(async cnn => await cnn.ExecuteAsync(@"UPDATE application.address SET street_name=@StreetName WHERE id=@Id",
                    new { entity.StreetName, address.Id }));
            }

            if (string.IsNullOrEmpty(address.Bag) && !string.IsNullOrEmpty(entity.Bag))
            {
                await RunSqlCommand(async cnn => await cnn.ExecuteAsync(@"UPDATE application.address SET bag=@Bag WHERE id=@Id",
                    new { entity.Bag, address.Id }));
            }

            return address;
        }

        public override Task<IReadOnlyList<Address>> ListAllAsync(Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            throw new NotImplementedException();
        }

        public override Task UpdateAsync(Address entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            throw new NotImplementedException();
        }
    }
}
