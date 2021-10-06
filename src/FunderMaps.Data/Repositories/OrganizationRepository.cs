using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace FunderMaps.Data.Repositories
{
    /// <summary>
    ///     Organization repository.
    /// </summary>
    internal class OrganizationRepository : RepositoryBase<Organization, Guid>, IOrganizationRepository
    {
        /// <summary>
        ///     Create new <see cref="Organization"/>.
        /// </summary>
        /// <remarks>
        ///     Organizations can only be created from organization
        ///     proposals. Therefore this method should be a no-op.
        /// </remarks>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created <see cref="Organization"/>.</returns>
        public override Task<Guid> AddAsync(Organization entity)
            => throw new InvalidOperationException();

        public async Task<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
        {
            var sql = @"
	            SELECT application.create_organization(
                    @id,
                    @email,
                    @passwordHash)";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);
            context.AddParameterWithValue("email", email);
            context.AddParameterWithValue("passwordHash", passwordHash);

            await using var reader = await context.ReaderAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override async Task<long> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.organization";

            await using var context = await DbContextFactory.CreateAsync(sql);

            return await context.ScalarAsync<long>();
        }

        /// <summary>
        ///     Delete <see cref="Organization"/>.
        /// </summary>
        /// <param name="id">Entity id.</param>
        public override async Task DeleteAsync(Guid id)
        {
            ResetCacheEntity(id);

            var sql = @"
                DELETE
                FROM    application.organization
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await context.NonQueryAsync();
        }

        private static void MapToWriter(DbContext context, Organization entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            context.AddParameterWithValue("phone_number", entity.PhoneNumber);
            context.AddParameterWithValue("branding_logo", entity.BrandingLogo);
            context.AddParameterWithValue("home_address", entity.HomeStreet);
            context.AddParameterWithValue("home_address_number", entity.HomeAddressNumber);
            context.AddParameterWithValue("home_address_number_postfix", entity.HomeAddressNumberPostfix);
            context.AddParameterWithValue("home_city", entity.HomeCity);
            context.AddParameterWithValue("home_postbox", entity.HomePostbox);
            context.AddParameterWithValue("home_zipcode", entity.HomeZipcode);
        }

        private static Organization MapFromReader(DbDataReader reader, int offset = 0)
            => new()
            {
                Id = reader.GetGuid(offset++),
                Name = reader.GetSafeString(offset++),
                Email = reader.GetSafeString(offset++),
                PhoneNumber = reader.GetSafeString(offset++),
                BrandingLogo = reader.GetSafeString(offset++),
                HomeStreet = reader.GetSafeString(offset++),
                HomeAddressNumber = reader.GetSafeInt(offset++),
                HomeAddressNumberPostfix = reader.GetSafeString(offset++),
                HomeCity = reader.GetSafeString(offset++),
                HomePostbox = reader.GetSafeString(offset++),
                HomeZipcode = reader.GetSafeString(offset++),
                Area = new()
                {
                    XMin = reader.GetSafeDouble(offset++),
                    YMin = reader.GetSafeDouble(offset++),
                    XMax = reader.GetSafeDouble(offset++),
                    YMax = reader.GetSafeDouble(offset++),
                },
                Center = new()
                {
                    CenterX = reader.GetSafeDouble(offset++),
                    CenterY = reader.GetSafeDouble(offset++),
                }
            };

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public override async Task<Organization> GetByIdAsync(Guid id)
        {
            if (TryGetEntity(id, out Organization entity))
            {
                return entity;
            }

            var sql = @"
                SELECT  id,
                        name,
                        email,
                        phone_number,
                        branding_logo,
                        home_address,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        st_xmin(fence) AS x_min,
                        st_ymin(fence) AS y_min,
                        st_xmax(fence) AS x_max,
                        st_ymax(fence) AS y_max,
                        st_x(st_centroid(fence)) AS center_x,
                        st_y(st_centroid(fence)) AS center_y
                FROM    application.organization
                WHERE   id = @id
                LIMIT   1";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", id);

            await using var reader = await context.ReaderAsync();

            return CacheEntity(MapFromReader(reader));
        }

        /// <summary>
        ///     Retrieve all <see cref="Organization"/>.
        /// </summary>
        /// <returns>List of <see cref="Organization"/>.</returns>
        public override async IAsyncEnumerable<Organization> ListAllAsync(Navigation navigation)
        {
            var sql = @"
                SELECT  id,
                        name,
                        email,
                        phone_number,
                        branding_logo,
                        home_address,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        st_xmin(fence) AS x_min,
                        st_ymin(fence) AS y_min,
                        st_xmax(fence) AS x_max,
                        st_ymax(fence) AS y_max,
                        st_x(st_centroid(fence)) AS center_x,
                        st_y(st_centroid(fence)) AS center_y
                FROM    application.organization";

            sql = ConstructNavigation(sql, navigation);

            await using var context = await DbContextFactory.CreateAsync(sql);

            await foreach (var reader in context.EnumerableReaderAsync())
            {
                yield return CacheEntity(MapFromReader(reader));
            }
        }

        /// <summary>
        ///     Update <see cref="Organization"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async Task UpdateAsync(Organization entity)
        {
            ResetCacheEntity(entity);

            var sql = @"
                UPDATE  application.organization
                SET     phone_number = @phone_number,
                        branding_logo = @branding_logo,
                        home_address = @home_address,
                        home_address_number = @home_address_number,
                        home_address_number_postfix = @home_address_number_postfix,
                        home_city = @home_city,
                        home_postbox = @home_postbox,
                        home_zipcode = @home_zipcode
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
