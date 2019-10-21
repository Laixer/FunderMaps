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
    /// Organization repository.
    /// </summary>
    public class OrganizationRepository : RepositoryBase<Organization, Guid>, IOrganizationRepository
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="dbProvider">Database provider.</param>
        public OrganizationRepository(DbProvider dbProvider) : base(dbProvider) { }

        // TODO: Move to own repo?
        /// <summary>
        /// Return all contractors.
        /// </summary>
        /// <returns>List of records.</returns>
        public async Task<IReadOnlyList<Contractor>> ListAllContractorsAsync(Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"
                SELECT contrctr.id,
                        contrctr.name
                FROM   application.contractor AS contrctr
                OFFSET @Offset
                LIMIT @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Contractor>(sql, navigation));
            if (!result.Any())
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Create new organization.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        /// <returns>Created entity primary key.</returns>
        public override Task<Guid> AddAsync(Organization entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                INSERT INTO application.organization
                        (name,
                        normalized_name,
                        email,
                        phone_number,
                        registration_number,
                        is_default,
                        is_validated,
                        branding_logo,
                        invoice_name,
                        invoice_po_number,
                        invoice_email,
                        home_address as home_street,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        home_state,
                        home_country,
                        postal_address as postal_street,
                        postal_address_number,
                        postal_address_number_postfix,
                        postal_city,
                        postal_postbox,
                        postal_zipcode,
                        postal_state,
                        postal_country)
                VALUES  (@Name,
                        @NormalizedName,
                        @Email,
                        @PhoneNumber,
                        @RegistrationNumber,
                        @IsDefault,
                        @IsValidated,
                        @BrandingLogo,
                        @InvoiceName,
                        @InvoicePONumber,
                        @InvoiceEmail
                        @HomeStreet,
                        @HomeAddressNumber,
                        @HomeAddressNumberPostfix,
                        @HomeCity,
                        @HomePostbox,
                        @HomeZipcode,
                        @HomeState,
                        @HomeCountry,
                        @PostalStreet,
                        @PostalAddressNumber,
                        @PostalAddressNumberPostfix,
                        @PostalCity,
                        @PostalPostbox,
                        @PostalZipcode,
                        @PostalState,
                        @PostalCountry)
                RETURNING id";

            return RunSqlCommand(async cnn => await cnn.ExecuteScalarAsync<Guid>(sql, entity));
        }

        /// <summary>
        /// Count entities.
        /// </summary>
        /// <returns>Number of records.</returns>
        public override Task<uint> CountAsync()
        {
            var sql = @"
                SELECT COUNT(*)
                FROM   application.organization";

            return RunSqlCommand(async cnn => await cnn.QuerySingleAsync<uint>(sql));
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public override Task DeleteAsync(Organization entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                DELET FROM application.organization AS org
                WHERE   org.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Unique identifier.</param>
        /// <returns><see cref="Organization"/> on success, null on error.</returns>
        public override async Task<Organization> GetByIdAsync(Guid id)
        {
            var sql = @"
                SELECT  id,
                        name,
                        normalized_name,
                        email,
                        phone_number,
                        registration_number,
                        is_default,
                        is_validated,
                        branding_logo,
                        invoice_name,
                        invoice_po_number,
                        invoice_email,
                        home_address as home_street,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        home_state,
                        home_country,
                        postal_address as postal_street,
                        postal_address_number,
                        postal_address_number_postfix,
                        postal_city,
                        postal_postbox,
                        postal_zipcode,
                        postal_state,
                        postal_country
                FROM    application.organization AS org
                WHERE   org.id = @Id
                LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Organization>(sql, new { Id = id }));
            if (!result.Any())
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Retrieve entity by name.
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <returns><see cref="Organization"/> on success, null on error.</returns>
        public async Task<Organization> GetByNormalizedNameAsync(string name)
        {
            var sql = @"
                SELECT  id,
                        name,
                        normalized_name,
                        email,
                        phone_number,
                        registration_number,
                        is_default,
                        is_validated,
                        branding_logo,
                        invoice_name,
                        invoice_po_number,
                        invoice_email,
                        home_address as home_street,
                        home_address_number,
                        home_address_number_postfix,
                        home_city,
                        home_postbox,
                        home_zipcode,
                        home_state,
                        home_country,
                        postal_address as postal_street,
                        postal_address_number,
                        postal_address_number_postfix,
                        postal_city,
                        postal_postbox,
                        postal_zipcode,
                        postal_state,
                        postal_country
                FROM    application.organization AS org
                WHERE   org.normalized_name = @NormalizedName
                LIMIT  1";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Organization>(sql, new { NormalizedName = name }));
            if (!result.Any())
            {
                return null;
            }

            return result.First();
        }

        /// <summary>
        /// Return all reports.
        /// </summary>
        /// <param name="navigation">Navigation options.</param>
        /// <returns>List of records.</returns>
        public override async Task<IReadOnlyList<Organization>> ListAllAsync(Navigation navigation)
        {
            if (navigation == null)
            {
                throw new ArgumentNullException(nameof(navigation));
            }

            var sql = @"SELECT  id,
                                name,
                                normalized_name,
                                email,
                                phone_number,
                                registration_number,
                                is_default,
                                is_validated,
                                branding_logo,
                                invoice_name,
                                invoice_po_number,
                                invoice_email,
                                home_address as home_street,
                                home_address_number,
                                home_address_number_postfix,
                                home_city,
                                home_postbox,
                                home_zipcode,
                                home_state,
                                home_country,
                                postal_address as postal_street,
                                postal_address_number,
                                postal_address_number_postfix,
                                postal_city,
                                postal_postbox,
                                postal_zipcode,
                                postal_state,
                                postal_country
                        FROM    application.organization
                        OFFSET  @Offset
                        LIMIT   @Limit";

            var result = await RunSqlCommand(async cnn => await cnn.QueryAsync<Organization>(sql, navigation));
            if (!result.Any())
            {
                return null;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        public override Task UpdateAsync(Organization entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var sql = @"
                UPDATE application.organization AS org
                SET    name = @Name,
                       email = @Email,
                       phone_number = @PhoneNumber,
                       registration_number = @RegistrationNumber,
                       is_default = @IsDefault,
                       is_validated = @IsValidated,
                       branding_logo = @BrandingLogo,
                       invoice_name = @InvoiceName,
                       invoice_po_number = @InvoicePONumber,
                       invoice_email = @InvoiceEmail,
                       home_address = @HomeStreet,
                       home_address_number = @HomeAddressNumber,
                       home_city = @HomeCity,
                       home_postbox = @HomePostbox,
                       home_zipcode = @HomeZipcode,
                       home_state = @HomeState,
                       home_country = @HomeCountry,
                       postal_address = @PostalStreet,
                       postal_address_number = @PostalAddressNumber,
                       postal_city = @PostalCity,
                       postal_postbox = @PostalPostbox,
                       postal_zipcode = @PostalZipcode,
                       postal_state = @PostalState,
                       postal_country = @PostalCountry
                WHERE  org.id = @Id";

            return RunSqlCommand(async cnn => await cnn.ExecuteAsync(sql, entity));
        }
    }
}
