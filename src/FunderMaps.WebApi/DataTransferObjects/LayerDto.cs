using FunderMaps.Core.Types.MapLayer;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Layer DTO.
    /// </summary>
    public class LayerDto
    {
        /// <summary>
        ///     Layer identifier.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     Layer full name
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        ///     Gets or sets the display name for the bundle.
        /// </summary>
        [Required]
        public string Name { get; set; }

    }
}
