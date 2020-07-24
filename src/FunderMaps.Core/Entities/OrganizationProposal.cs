using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Organization proposal.
    /// </summary>
    public sealed class OrganizationProposal : BaseEntity
    {
        /// <summary>
        ///     Organization identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Proposed organization name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
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
        public void InitializeDefaults()
        {
            Id = Guid.Empty;
        }
    }
}
