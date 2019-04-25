using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using FunderMaps.Core.Interfaces;

namespace FunderMaps.Core.Entities.Fis
{
    public class Sample : AccessControl, ISoftDeletable
    {
        public int Id { get; set; }

        [Required]
        public int? Report { get; set; }

        public FoundationType FoundationType { get; set; }

        public FoundationQuality? FoundationQuality { get; set; }
        public Substructure Substructure { get; set; }

        public string MonitoringWell { get; set; }
        public string Cpt { get; set; }
        public string Note { get; set; }
        public decimal? WoodLevel { get; set; }
        public decimal? GroundwaterLevel { get; set; }
        public decimal? GroundLevel { get; set; }
        public bool FoundationRecoveryAdviced { get; set; }

        public FoundationDamageCause FoundationDamageCause { get; set; }

        public short? BuiltYear { get; set; }

        [IgnoreDataMember]
        public Guid _Address { get; set; }

        public EnforcementTerm EnforcementTerm { get; set; }

        [IgnoreDataMember]
        public string _BaseMeasurementLevel { get; set; }

        [Required]
        public virtual Address Address { get; set; }

        public virtual BaseLevel BaseMeasurementLevel { get; set; }

        [IgnoreDataMember]
        public virtual Report ReportNavigation { get; set; }
    }
}
