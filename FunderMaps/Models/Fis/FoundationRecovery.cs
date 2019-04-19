using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models.Fis
{
    public class FoundationRecovery : RecordControl
    {
        public FoundationRecovery()
        {
            FoundationRecoveryEvidence = new HashSet<FoundationRecoveryEvidence>();
            FoundationRecoveryRepair = new HashSet<FoundationRecoveryRepair>();
        }

        public int Id { get; set; }

        public string Note { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public short? Year { get; set; }

        [Required]
        public Guid Address { get; set; }

        [Required]
        public string AccessPolicy { get; set; }

        [Required]
        public int Attribution { get; set; }

        [IgnoreDataMember]
        public virtual AccessPolicy AccessPolicyNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Address AddressNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Attribution AttributionNavigation { get; set; }

        [IgnoreDataMember]
        public virtual FoundationRecoveryType TypeNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecoveryEvidence> FoundationRecoveryEvidence { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecoveryRepair> FoundationRecoveryRepair { get; set; }
    }
}
