using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Contractor DTO.
    /// </summary>
    public class ContractorDto
    {
        /// <summary>
        ///     Contractor identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the name for the contractor.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
