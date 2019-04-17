using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models
{
    public class Organization
    {
        public Organization()
        {
        }

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
        public int HomeAddressId { get; set; }

        [IgnoreDataMember]
        public int PostalAddressId { get; set; }

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

        [Required]
        public virtual Address HomeAddress { get; set; }

        [Required]
        public virtual Address PostalAddres { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
