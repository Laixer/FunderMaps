using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Organization DTO.
    /// </summary>
    public class OrganizationDto
    {
        /// <summary>
        ///     Organization identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the name for the organization.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the email address for the organization.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Organizatoin registration number.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        ///     Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; set; }

        /// <summary>
        ///     Name on the invoice.
        /// </summary>
        public string InvoiceName { get; set; }

        /// <summary>
        ///     Invoice PB box number.
        /// </summary>
        public string InvoicePoBox { get; set; }

        /// <summary>
        ///     Send invoices to designated email address.
        /// </summary>
        public string InvoiceEmail { get; set; }

        // TODO: Convert to geocoder address?

        /// <summary>
        ///     Street.
        /// </summary>
        public string HomeStreet { get; set; }

        /// <summary>
        ///     Street number.
        /// </summary>
        public int? HomeAddressNumber { get; set; }

        /// <summary>
        ///     Address number postfix.
        /// </summary>
        public string HomeAddressNumberPostfix { get; set; }

        /// <summary>
        ///     City.
        /// </summary>
        public string HomeCity { get; set; }

        /// <summary>
        ///     Postbox number.
        /// </summary>
        public string HomePostbox { get; set; }

        /// <summary>
        ///     Zipcode or postcode.
        /// </summary>
        public string HomeZipcode { get; set; }

        /// <summary>
        ///     State or province.
        /// </summary>
        public string HomeState { get; set; }

        /// <summary>
        ///     Country.
        /// </summary>
        public string HomeCountry { get; set; }

        /// <summary>
        ///     Street.
        /// </summary>
        public string PostalStreet { get; set; }

        /// <summary>
        ///     Street number.
        /// </summary>
        public int? PostalAddressNumber { get; set; }

        /// <summary>
        ///     Address number postfix.
        /// </summary>
        public string PostalAddressNumberPostfix { get; set; }

        /// <summary>
        ///     City.
        /// </summary>
        public string PostalCity { get; set; }

        /// <summary>
        ///     Postbox number.
        /// </summary>
        public string PostalPostbox { get; set; }

        /// <summary>
        ///     Zipcode or postcode.
        /// </summary>
        public string PostalZipcode { get; set; }

        /// <summary>
        ///     State or province.
        /// </summary>
        public string PostalState { get; set; }

        /// <summary>
        ///     Country.
        /// </summary>
        public string PostalCountry { get; set; }
    }
}
