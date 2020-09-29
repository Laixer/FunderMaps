using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Extensions;
using FunderMaps.Data.Providers;
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
        public override ValueTask<Guid> AddAsync(Organization entity)
        {
            throw new InvalidOperationException();
        }

        public async ValueTask<Guid> AddFromProposalAsync(Guid id, string email, string passwordHash)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            // TODO: normalized_email should be db trigger function
            var sql = @"
	            SELECT application.create_organization(
                    @id,
                    @email,
                    @passwordHash)";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            cmd.AddParameterWithValue("id", id);
            cmd.AddParameterWithValue("email", email);
            cmd.AddParameterWithValue("passwordHash", passwordHash);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetGuid(0);
        }

        /// <summary>
        ///     Retrieve number of entities.
        /// </summary>
        /// <returns>Number of entities.</returns>
        public override ValueTask<ulong> CountAsync()
        {
            var sql = @"
                SELECT  COUNT(*)
                FROM    application.organization";

            return ExecuteScalarUnsignedLongCommandAsync(sql);
        }

        /// <summary>
        ///     Delete <see cref="Organization"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask DeleteAsync(Guid id)
        {
            var sql = @"
                DELETE
                FROM    application.organization
                WHERE   id = @id";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);
            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }

        private static void MapToWriter(DbCommand cmd, Organization entity)
        {
            cmd.AddParameterWithValue("phone_number", entity.PhoneNumber);
            cmd.AddParameterWithValue("registration_number", entity.RegistrationNumber);
            cmd.AddParameterWithValue("branding_logo", entity.BrandingLogo);
            cmd.AddParameterWithValue("invoice_name", entity.InvoiceName);
            cmd.AddParameterWithValue("invoice_po_box", entity.InvoicePoBox);
            cmd.AddParameterWithValue("invoice_email", entity.InvoiceEmail);
            cmd.AddParameterWithValue("home_address", entity.HomeStreet);
            cmd.AddParameterWithValue("home_address_number", entity.HomeAddressNumber);
            cmd.AddParameterWithValue("home_address_number_postfix", entity.HomeAddressNumberPostfix);
            cmd.AddParameterWithValue("home_city", entity.HomeCity);
            cmd.AddParameterWithValue("home_postbox", entity.HomePostbox);
            cmd.AddParameterWithValue("home_zipcode", entity.HomeZipcode);
            cmd.AddParameterWithValue("home_state", entity.HomeState);
            cmd.AddParameterWithValue("home_country", entity.HomeCountry);
            cmd.AddParameterWithValue("postal_address", entity.PostalStreet);
            cmd.AddParameterWithValue("postal_address_number", entity.PostalAddressNumber);
            cmd.AddParameterWithValue("postal_address_number_postfix", entity.PostalAddressNumberPostfix);
            cmd.AddParameterWithValue("postal_city", entity.PostalCity);
            cmd.AddParameterWithValue("postal_postbox", entity.PostalPostbox);
            cmd.AddParameterWithValue("postal_zipcode", entity.PostalZipcode);
            cmd.AddParameterWithValue("postal_state", entity.PostalState);
            cmd.AddParameterWithValue("postal_country", entity.PostalCountry);
        }

        private static Organization MapFromReader(DbDataReader reader)
            => new Organization
            {
                Id = reader.GetGuid(0),
                Name = reader.GetSafeString(1),
                Email = reader.GetSafeString(2),
                PhoneNumber = reader.GetSafeString(3),
                RegistrationNumber = reader.GetSafeString(4),
                BrandingLogo = reader.GetSafeString(5),
                InvoiceName = reader.GetSafeString(6),
                InvoicePoBox = reader.GetSafeString(7),
                InvoiceEmail = reader.GetSafeString(8),
                HomeStreet = reader.GetSafeString(9),
                HomeAddressNumber = reader.GetSafeInt(10),
                HomeAddressNumberPostfix = reader.GetSafeString(11),
                HomeCity = reader.GetSafeString(12),
                HomePostbox = reader.GetSafeString(13),
                HomeZipcode = reader.GetSafeString(14),
                HomeState = reader.GetSafeString(15),
                HomeCountry = reader.GetSafeString(16),
                PostalStreet = reader.GetSafeString(17),
                PostalAddressNumber = reader.GetSafeInt(18),
                PostalAddressNumberPostfix = reader.GetSafeString(19),
                PostalCity = reader.GetSafeString(20),
                PostalPostbox = reader.GetSafeString(21),
                PostalZipcode = reader.GetSafeString(22),
                PostalState = reader.GetSafeString(23),
                PostalCountry = reader.GetSafeString(24),
            };

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public override async ValueTask<Organization> GetByIdAsync(Guid id)
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
                FROM    application.organization
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by email and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public async ValueTask<Organization> GetByNameAsync(string name)
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
                FROM    application.organization
                WHERE   normalized_name = application.normalize(@name)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("name", name);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        /// <summary>
        ///     Retrieve <see cref="Organization"/> by email and password hash.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/>.</returns>
        public async ValueTask<Organization> GetByEmailAsync(string email)
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
                FROM    application.organization
                WHERE   normalized_email = application.normalize(@email)
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("email", email);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return MapFromReader(reader);
        }

        public async ValueTask<string> GetFenceAsync(Organization entity)
        {
            var sql = @"
                SELECT  ST_AsText(fence) AS fence
                FROM    application.organization
                WHERE   id = @id
                LIMIT   1";

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            await using var reader = await cmd.ExecuteReaderAsyncEnsureRowAsync();
            await reader.ReadAsync();

            return reader.GetSafeString(0);
        }

        /// <summary>
        ///     Retrieve all <see cref="Organization"/>.
        /// </summary>
        /// <returns>List of <see cref="Organization"/>.</returns>
        public override async IAsyncEnumerable<Organization> ListAllAsync(INavigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
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
                FROM    application.organization";

            ConstructNavigation(ref sql, navigation);

            await using var connection = await DbProvider.OpenConnectionScopeAsync();
            await using var cmd = DbProvider.CreateCommand(sql, connection);

            await using var reader = await cmd.ExecuteReaderCanHaveZeroRowsAsync();
            while (await reader.ReadAsync())
            {
                yield return MapFromReader(reader);
            }
        }

        /// <summary>
        ///     Update <see cref="Organization"/>.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        public override async ValueTask UpdateAsync(Organization entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

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

            using var connection = await DbProvider.OpenConnectionScopeAsync();
            using var cmd = DbProvider.CreateCommand(sql, connection);
            cmd.AddParameterWithValue("id", entity.Id);

            MapToWriter(cmd, entity);

            await cmd.ExecuteNonQueryEnsureAffectedAsync();
        }
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
