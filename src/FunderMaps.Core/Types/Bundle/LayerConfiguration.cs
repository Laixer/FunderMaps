using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Types.MapLayer
{
    /// <summary>
    ///     Wrapper around the layer configuration for a bundle.
    /// </summary>
    public sealed class LayerConfiguration
    {
        /// <summary>
        ///     Collection of layer column configurations.
        /// </summary>
        [Required]
        public IEnumerable<LayerColumnPair> Layers { get; set; }
    }
}
