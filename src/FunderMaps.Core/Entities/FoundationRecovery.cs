using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    public class FoundationRecovery : RecordControl
    {
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
