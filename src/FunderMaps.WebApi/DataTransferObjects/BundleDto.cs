using FunderMaps.Core.Types.MapLayer;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.WebApi.DataTransferObjects
{
    /// <summary>
    ///     Bundle DTO.
    /// </summary>
    public class BundleDto
    {
        /// <summary>
        ///     Bundle identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     Organization identifier.
        /// </summary>
        [Required]
        public Guid OrganizationId { get; set; }

        /// <summary>
        ///     Bundle layer configuration.
        /// </summary>
        public LayerConfiguration LayerConfiguration { get; set; }

        /// <summary>
        ///     Gets or sets the display name for the bundle.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     Bundle creation date.
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///     Bundle update date.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        ///     Bundle deletion date.
        /// </summary>
        public DateTime? DeleteDate { get; set; }
    }
}
