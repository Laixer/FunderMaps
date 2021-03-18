using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Organization proposal.
    /// </summary>
    public sealed class OrganizationProposal : IdentifiableEntity<OrganizationProposal, Guid>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationProposal()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Organization identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Proposed organization name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Proposed organization email.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing organization.</returns>
        public override string ToString() => Name;

        /// <summary>
        ///     Initialize property defaults.
        /// </summary>
        public override void InitializeDefaults()
        {
            Id = Guid.Empty;
        }
    }
}
