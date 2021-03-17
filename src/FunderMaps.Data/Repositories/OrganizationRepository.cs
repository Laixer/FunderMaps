using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

#pragma warning disable CA1812 // Internal class is never instantiated
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
            context.AddParameterWithValue("registration_number", entity.RegistrationNumber);
            context.AddParameterWithValue("branding_logo", entity.BrandingLogo);
            context.AddParameterWithValue("invoice_name", entity.InvoiceName);
            context.AddParameterWithValue("invoice_po_box", entity.InvoicePoBox);
            context.AddParameterWithValue("invoice_email", entity.InvoiceEmail);
            context.AddParameterWithValue("home_address", entity.HomeStreet);
            context.AddParameterWithValue("home_address_number", entity.HomeAddressNumber);
            context.AddParameterWithValue("home_address_number_postfix", entity.HomeAddressNumberPostfix);
            context.AddParameterWithValue("home_city", entity.HomeCity);
            context.AddParameterWithValue("home_postbox", entity.HomePostbox);
            context.AddParameterWithValue("home_zipcode", entity.HomeZipcode);
            context.AddParameterWithValue("home_state", entity.HomeState);
            context.AddParameterWithValue("home_country", entity.HomeCountry);
            context.AddParameterWithValue("postal_address", entity.PostalStreet);
            context.AddParameterWithValue("postal_address_number", entity.PostalAddressNumber);
            context.AddParameterWithValue("postal_address_number_postfix", entity.PostalAddressNumberPostfix);
            context.AddParameterWithValue("postal_city", entity.PostalCity);
            context.AddParameterWithValue("postal_postbox", entity.PostalPostbox);
            context.AddParameterWithValue("postal_zipcode", entity.PostalZipcode);
            context.AddParameterWithValue("postal_state", entity.PostalState);
            context.AddParameterWithValue("postal_country", entity.PostalCountry);
        }

        private static Organization MapFromReader(DbDataReader reader, bool fullMap = false, int offset = 0)
            => new()
            {
                Id = reader.GetGuid(offset + 0),
                Name = reader.GetSafeString(offset + 1),
                Email = reader.GetSafeString(offset + 2),
                PhoneNumber = reader.GetSafeString(offset + 3),
                RegistrationNumber = reader.GetSafeString(offset + 4),
                BrandingLogo = reader.GetSafeString(offset + 5),
                InvoiceName = reader.GetSafeString(offset + 6),
                InvoicePoBox = reader.GetSafeString(offset + 7),
                InvoiceEmail = reader.GetSafeString(offset + 8),
                HomeStreet = reader.GetSafeString(offset + 9),
                HomeAddressNumber = reader.GetSafeInt(offset + 10),
                HomeAddressNumberPostfix = reader.GetSafeString(offset + 11),
                HomeCity = reader.GetSafeString(offset + 12),
                HomePostbox = reader.GetSafeString(offset + 13),
                HomeZipcode = reader.GetSafeString(offset + 14),
                HomeState = reader.GetSafeString(offset + 15),
                HomeCountry = reader.GetSafeString(offset + 16),
                PostalStreet = reader.GetSafeString(offset + 17),
                PostalAddressNumber = reader.GetSafeInt(offset + 18),
                PostalAddressNumberPostfix = reader.GetSafeString(offset + 19),
                PostalCity = reader.GetSafeString(offset + 20),
                PostalPostbox = reader.GetSafeString(offset + 21),
                PostalZipcode = reader.GetSafeString(offset + 22),
                PostalState = reader.GetSafeString(offset + 23),
                PostalCountry = reader.GetSafeString(offset + 24),
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
                        registration_number,
                        branding_logo,
                        invoice_name,
                        invoice_po_box,
                        invoice_email,
                        home_address,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        home_state,
                        home_country,
                        postal_address,
                        postal_address_number,
                        postal_address_number_postfix,
                        postal_city,
                        postal_postbox,
                        postal_zipcode,
                        postal_state,
                        postal_country
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
                        registration_number,
                        branding_logo,
                        invoice_name,
                        invoice_po_box,
                        invoice_email,
                        home_address,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        home_state,
                        home_country,
                        postal_address,
                        postal_address_number,
                        postal_address_number_postfix,
                        postal_city,
                        postal_postbox,
                        postal_zipcode,
                        postal_state,
                        postal_country
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
                        registration_number = @registration_number,
                        branding_logo = @branding_logo,
                        invoice_name = @invoice_name,
                        invoice_po_box = @invoice_po_box,
                        invoice_email = @invoice_email,
                        home_address = @home_address,
                        home_address_number = @home_address_number,
                        home_address_number_postfix = @home_address_number_postfix,
                        home_city = @home_city,
                        home_postbox = @home_postbox,
                        home_zipcode = @home_zipcode,
                        home_state = @home_state,
                        home_country = @home_country,
                        postal_address = @postal_address,
                        postal_address_number = @postal_address_number,
                        postal_address_number_postfix = @postal_address_number_postfix,
                        postal_city = @postal_city,
                        postal_postbox = @postal_postbox,
                        postal_zipcode = @postal_zipcode,
                        postal_state = @postal_state,
                        postal_country = @postal_country
                WHERE   id = @id";

            await using var context = await DbContextFactory.CreateAsync(sql);

            context.AddParameterWithValue("id", entity.Id);

            MapToWriter(context, entity);

            await context.NonQueryAsync();
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
