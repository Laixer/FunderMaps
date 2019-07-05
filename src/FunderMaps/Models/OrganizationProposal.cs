using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models
{
    public class OrganizationProposal
    {
        public Guid Token { get; set; }

        /// <summary>
        /// Proposed organization name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for the organization proposal.
        /// </summary>
        [IgnoreDataMember]
        public virtual string NormalizedName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public OrganizationProposal(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
