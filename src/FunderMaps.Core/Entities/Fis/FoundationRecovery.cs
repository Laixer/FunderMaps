using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    public class FoundationRecovery : RecordControl
    {
        /// <summary>
        /// Create new instance.
        /// </summary>
        public FoundationRecovery()
        {
            FoundationRecoveryEvidence = new HashSet<FoundationRecoveryEvidence>();
            FoundationRecoveryRepair = new HashSet<FoundationRecoveryRepair>();
        }

        public int Id { get; set; }

        public string Note { get; set; }

        [Required]
        public FoundationRecoveryType Type { get; set; }

        [Required]
        public short Year { get; set; }

        [Required]
        public Guid Address { get; set; }

        public AccessPolicy AccessPolicy { get; set; }

        [Required]
        public int Attribution { get; set; }

        [IgnoreDataMember]
        public virtual Address AddressNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Attribution AttributionNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecoveryEvidence> FoundationRecoveryEvidence { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecoveryRepair> FoundationRecoveryRepair { get; set; }
    }
}
