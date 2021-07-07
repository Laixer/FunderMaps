using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Webservice.DataTransferObjects
{
    /// <summary>
    ///     Contractor DTO.
    /// </summary>
    public record ProductTelemetryDto
    {
        /// <summary>
        ///     Product name.
        /// </summary>
        [Required]
        public string Product { get; init; }

        /// <summary>
        ///     Product hit count.
        /// </summary>
        public int Count { get; init; }
    }
}
