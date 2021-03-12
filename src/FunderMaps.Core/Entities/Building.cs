using FunderMaps.Core.DataAnnotations;
using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Building entity.
    /// </summary>
    public sealed class Building : IdentifiableEntity<Building, string>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Building()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Required, Geocoder]
        public string Id { get; set; }

        // FUTURE: Contraints, see #203
        /// <summary>
        ///     Built year.
        /// </summary>
        public DateTime? BuiltYear { get; set; }

        /// <summary>
        ///     Building is active or not.
        /// </summary>
        [Required]
        public bool IsActive { get; set; } = true;

        /// <summary>
        ///     Geometry.
        /// </summary>
        [Required]
        public string Geometry { get; set; }

        // TODO: Type, see #211
        /// <summary>
        ///     External data source id.
        /// </summary>
        [Required]
        public string ExternalId { get; set; }

        /// <summary>
        ///     External data source.
        /// </summary>
        [Required]
        public ExternalDataSource ExternalSource { get; set; }

        /// <summary>
        ///     Building type.
        /// </summary>
        public BuildingType? BuildingType { get; set; }

        /// <summary>
        ///     Neighborhood identifier.
        /// </summary>
        [Geocoder]
        public string NeighborhoodId { get; set; }
    }
}
