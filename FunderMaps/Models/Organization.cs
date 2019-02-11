using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models
{
    public class Organization
    {
        public Organization()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }

        [IgnoreDataMember]
        public int HomeAddressId { get; set; }

        [IgnoreDataMember]
        public int PostalAddressId { get; set; }

        [IgnoreDataMember]
        public bool IsDefault { get; set; }

        [IgnoreDataMember]
        public bool IsValidated { get; set; }

        public string BrandingLogo { get; set; }

        [IgnoreDataMember]
        public int AttestationOrganizationId { get; set; }

        [Required]
        public virtual Address HomeAddress { get; set; }

        [Required]
        public virtual Address PostalAddres { get; set; }
    }
}
