using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Types.BundleLayers
{
    /// <summary>
    ///     Wrapper around a layer and it's columns.
    /// </summary>
    public sealed class LayerColumnPair
    {
        /// <summary>
        ///     Represents the layer id.
        /// </summary>
        [Required]
        public Guid LayerId { get; set; }

        // TODO Min length attribute?
        // TODO Must contain geom attribute?
        /// <summary>
        ///     Collection of selected column names.
        /// </summary>
        [Required]
        public IEnumerable<string> ColumnNames { get; set; }
    }
}
