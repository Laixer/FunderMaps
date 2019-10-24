using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Indicent entity.
    /// </summary>
    public class Incident : BaseEntity
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Foundation type.
        /// </summary>
        public FoundationType FoundationType { get; set; }

        /// <summary>
        /// Foundation quality.
        /// </summary>
        public FoundationQuality? FoundationQuality { get; set; }

        /// <summary>
        /// Substructure.
        /// </summary>
        public Substructure Substructure { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Foundation damage cause.
        /// </summary>
        public FoundationDamageCause FoundationDamageCause { get; set; }

        /// <summary>
        /// Address identifier.
        /// </summary>
        public Guid Address { get; set; }

        /// <summary>
        /// Owner user identifier.
        /// </summary>
        public Guid Owner { get; set; }

        /// <summary>
        /// Document name.
        /// </summary>
        [MaxLength(256)]
        public string DocumentName { get; set; }

        /// <summary>
        /// Address object.
        /// </summary>
        public Address AddressNavigation { get; set; }

        // TODO: Replace by user object.
        /// <summary>
        /// Owner object.
        /// </summary>
        public object OwnerNavigation { get; set; }
    }
}
