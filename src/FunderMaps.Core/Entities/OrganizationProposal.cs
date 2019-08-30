using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Organization proposal.
    /// </summary>
    public class OrganizationProposal : BaseEntity
    {
        /// <summary>
        /// Unique token.
        /// </summary>
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

        /// <summary>
        /// Proposed organization email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="name">Organization name.</param>
        /// <param name="email">Organization email.</param>
        public OrganizationProposal(string name, string email)
        {
            Name = name;
            Email = email;
        }

        /// <summary>
        /// Organization as string.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString() => Name;
    }
}
