using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Types.MapLayer
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

        /// <summary>
        ///     Collection of selected column names.
        /// </summary>
        [Required]
        public IEnumerable<string> ColumnNames { get; set; }
    }
}
