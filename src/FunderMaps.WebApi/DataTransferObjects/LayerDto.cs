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
        ///     The full layer name including the schema, appended with an underscore.
        /// </summary>
        [Required]
        public string Slug { get; set; }

        /// <summary>
        ///     Gets or sets the display name for the bundle.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
