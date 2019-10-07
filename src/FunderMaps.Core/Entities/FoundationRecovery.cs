using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Foundation recovery entity.
    /// </summary>
    public class FoundationRecovery : AttributionControl
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Foundation recovery type.
        /// </summary>
        [Required]
        public FoundationRecoveryType Type { get; set; }

        /// <summary>
        /// Year.
        /// </summary>
        [Required]
        public short Year { get; set; }

        /// <summary>
        /// Foundation recovery.
        /// </summary>
        public FoundationRecoveryLocation[] Recovery { get; set; }

        /// <summary>
        /// Address record identifier.
        /// </summary>
        [IgnoreDataMember]
        public Guid _Address { get; set; }

        /// <summary>
        /// Associated address.
        /// </summary>
        [Required]
        public Address Address { get; set; }
    }
}
