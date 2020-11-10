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
        ///     The schema name of the table referenced by this layer.
        /// </summary>
        [Required]
        public string SchemaName { get; set; }

        /// <summary>
        ///     The name of the table referenced by this layer.
        /// </summary>
        [Required]
        public string TableName { get; set; }

        /// <summary>
        ///     Get the full class name including the schema.
        /// </summary>
        public string FullName => !string.IsNullOrEmpty(SchemaName) ? $"{SchemaName}.{TableName}" : TableName;
    }
}
