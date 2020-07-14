using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    /// Address repository.
    /// </summary>
    internal class AddressRepository : RepositoryBase<Address, string>, IAddressRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public AddressRepository(DbProvider dbProvider) : base(dbProvider) { }

        /// <summary>
        /// Create new <see cref="Address"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Address"/>.</returns>
        public override async ValueTask<string> AddAsync(Address entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    INSERT INTO geocoder.address(
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source)
                    VALUES (
                        @building_number,
                        @postal_code,
                        @street,
                        @is_active,
                        @external_id,
                        @external_source)
                    ON CONFLICT DO NOTHING
                    RETURNING id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("building_number", entity.BuildingNumber);
            cmd.AddParameterWithValue("postal_code", entity.PostalCode);
            cmd.AddParameterWithValue("street", entity.Street);
            cmd.AddParameterWithValue("is_active", entity.IsActive);
            cmd.AddParameterWithValue("external_id", entity.ExternalId);
            cmd.AddParameterWithValue("external_source", entity.ExternalSource);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return reader.SafeGetString(0);
        }

        /// <summary>
        /// Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    geocoder.address";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        /// Delete <see cref="Incident"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override ValueTask DeleteAsync(string id) => new ValueTask();

        public async Task<Address> GetByExternalIdAsync(string id, string source)
        {
            var sql = @"
                SELECT  id,
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source
                FROM    geocoder.address
                WHERE   external_id = @external_id
                AND     external_source = @external_source
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("external_id", id);
            cmd.AddParameterWithValue("external_source", source);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return new Address
            {
                Id = reader.SafeGetString(0),
                BuildingNumber = reader.SafeGetString(1),
                PostalCode = reader.SafeGetString(2),
                Street = reader.SafeGetString(3),
                IsActive = reader.GetBoolean(4),
                ExternalId = reader.SafeGetString(5),
                ExternalSource = reader.SafeGetString(6),
            };
        }

        /// <summary>
        /// Retrieve <see cref="Address"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Address"/>.</returns>
        public override async ValueTask<Address> GetByIdAsync(string id)
        {
            var sql = @"
                SELECT  id,
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source
                FROM    geocoder.address
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            await reader.ReadAsync().ConfigureAwait(false);

            return new Address
            {
                Id = reader.SafeGetString(0),
                BuildingNumber = reader.SafeGetString(1),
                PostalCode = reader.SafeGetString(2),
                Street = reader.SafeGetString(3),
                IsActive = reader.GetBoolean(4),
                ExternalId = reader.SafeGetString(5),
                ExternalSource = reader.SafeGetString(6),
            };
        }

        /// <summary>
        /// Retrieve all <see cref="Address"/> matching postal code.
        /// </summary>
        /// <returns>List of <see cref="Address"/>.</returns>
        public async IAsyncEnumerable<Address> GetByPostalCodeAsync(string postalCode, INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source
                FROM    geocoder.address
                WHERE   postal_code = @postal_code";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("postal_code", postalCode);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                yield return new Address
                {
                    Id = reader.SafeGetString(0),
                    BuildingNumber = reader.SafeGetString(1),
                    PostalCode = reader.SafeGetString(2),
                    Street = reader.SafeGetString(3),
                    IsActive = reader.GetBoolean(4),
                    ExternalId = reader.SafeGetString(5),
                    ExternalSource = reader.SafeGetString(6),
                };
            }
        }

        // TODO: Combination should be unique
        /// <summary>
        /// Retrieve <see cref="Address"/> by postal code and building number.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Address"/>.</returns>
        public async IAsyncEnumerable<Address> GetByPostalCodeAsync(string postalCode, string buildingNumber, INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source
                FROM    geocoder.address
                WHERE   postal_code = @postal_code
                AND     building_number = @building_number";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("postal_code", postalCode);
            cmd.AddParameterWithValue("building_number", buildingNumber);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                yield return new Address
                {
                    Id = reader.SafeGetString(0),
                    BuildingNumber = reader.SafeGetString(1),
                    PostalCode = reader.SafeGetString(2),
                    Street = reader.SafeGetString(3),
                    IsActive = reader.GetBoolean(4),
                    ExternalId = reader.SafeGetString(5),
                    ExternalSource = reader.SafeGetString(6),
                };
            }
        }

        /// <summary>
        /// Retrieve all <see cref="Address"/>.
        /// </summary>
        /// <returns>List of <see cref="Address"/>.</returns>
        public override async IAsyncEnumerable<Address> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT  id,
                        building_number,
                        postal_code,
                        street,
                        is_active,
                        external_id,
                        external_source
                FROM    geocoder.address";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                yield return new Address
                {
                    Id = reader.SafeGetString(0),
                    BuildingNumber = reader.SafeGetString(1),
                    PostalCode = reader.SafeGetString(2),
                    Street = reader.SafeGetString(3),
                    IsActive = reader.GetBoolean(4),
                    ExternalId = reader.SafeGetString(5),
                    ExternalSource = reader.SafeGetString(6),
                };
            }
        }

        /// <summary>
        /// Update <see cref="Address"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Address entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                    UPDATE  geocoder.address
                    SET     building_number = @building_number,
                            postal_code = @postal_code,
                            street = @street,
                            is_active = @is_active,
                            external_id = @external_id,
                            external_source = @external_source
                    WHERE   id = @id";

            using var connection = await DbProvider.OpenConnectionScopeAsync().ConfigureAwait(false);
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("building_number", entity.BuildingNumber);
            cmd.AddParameterWithValue("postal_code", entity.PostalCode);
            cmd.AddParameterWithValue("street", entity.Street);
            cmd.AddParameterWithValue("is_active", entity.IsActive);
            cmd.AddParameterWithValue("external_id", entity.ExternalId);
            cmd.AddParameterWithValue("external_source", entity.ExternalSource);
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
