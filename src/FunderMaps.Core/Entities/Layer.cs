using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Entity representing a map layer.
    /// </summary>
    public sealed class Layer : IdentifiableEntity<Layer, Guid>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Layer()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     The layer name.
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
