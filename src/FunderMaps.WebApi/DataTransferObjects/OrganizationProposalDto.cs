using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Organization proposal DTO.
    /// </summary>
    public sealed class OrganizationProposalDto
    {
        /// <summary>
        ///     Organization identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Organization name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Organization email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
