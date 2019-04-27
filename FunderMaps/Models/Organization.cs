using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models
{
    public class Organization
    {
        /// <summary>
        /// Organization identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name for the organization.
        /// </summary>
        [PersonalData]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for the organization.
        /// </summary>
        [PersonalData]
        [IgnoreDataMember]
        public virtual string NormalizedName { get; set; }

        /// <summary>
        /// Gets or sets the email address for the organization.
        /// </summary>
        public string Email { get; set; }

        [FromQuery(Name = "phone_number")]
        public string PhoneNumber { get; set; }

        [FromQuery(Name = "registration_number")]
        public string RegistrationNumber { get; set; }

        [IgnoreDataMember]
        public bool? IsDefault { get; set; }

        [IgnoreDataMember]
        public bool? IsValidated { get; set; }

        /// <summary>
        /// Gets or sets the organization logo.
        /// </summary>
        public string BrandingLogo { get; set; }

        /// <summary>
        /// Name on the invoice.
        /// </summary>
        public string InvoiceName { get; set; }

        /// <summary>
        /// Invoice PB box number.
        /// </summary>
        public string InvoicePONumber { get; set; }

        /// <summary>
        /// Send invoices to designated email address.
        /// </summary>
        public string InvoiceEmail { get; set; }

        [IgnoreDataMember]
        public int AttestationOrganizationId { get; set; }

        public string HomeStreet { get; set; }
        public int? HomeAddressNumber { get; set; }
        public string HomeAddressNumberPostfix { get; set; }
        public string HomeCity { get; set; }
        public string HomePostbox { get; set; }
        public string HomeZipcode { get; set; }
        public string HomeState { get; set; }
        public string HomeCountry { get; set; }

        public string PostalStreet { get; set; }
        public int? PostalAddressNumber { get; set; }
        public string PostalAddressNumberPostfix { get; set; }
        public string PostalCity { get; set; }
        public string PostalPostbox { get; set; }
        public string PostalZipcode { get; set; }
        public string PostalState { get; set; }
        public string PostalCountry { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
