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

        [IgnoreDataMember]
        public string _FoundationType { get; set; }

        [IgnoreDataMember]
        public string _FoundationQuality { get; set; }

        [IgnoreDataMember]
        public string _Substructure { get; set; }

        public string MonitoringWell { get; set; }
        public string Cpt { get; set; }
        public string Note { get; set; }
        public decimal? WoodLevel { get; set; }
        public decimal? GroundwaterLevel { get; set; }
        public decimal? GroundLevel { get; set; }
        public bool FoundationRecoveryAdviced { get; set; }

        [IgnoreDataMember]
        public string _FoundationDamageCause { get; set; }

        public short? BuiltYear { get; set; }

        [IgnoreDataMember]
        public Guid _Address { get; set; }

        [IgnoreDataMember]
        public string _EnforcementTerm { get; set; }

        [IgnoreDataMember]
        public string _BaseMeasurementLevel { get; set; }

        [Required]
        public virtual Address Address { get; set; }

        public virtual BaseLevel BaseMeasurementLevel { get; set; }

        public virtual EnforcementTerm EnforcementTerm { get; set; }

        public virtual FoundationDamageCause FoundationDamageCause { get; set; }

        public virtual FoundationQuality FoundationQuality { get; set; }

        public virtual FoundationType FoundationType { get; set; }

        [IgnoreDataMember]
        public virtual Report ReportNavigation { get; set; }

        public virtual Substructure Substructure { get; set; }
    }
}
