using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class Sample : AccessControl, ISoftDeletable
    {
        public int Id { get; set; }

        [Required]
        public int? Report { get; set; }

        public string _FoundationType { get; set; }
        public string _FoundationQuality { get; set; }
        public string _Substructure { get; set; }
        public string MonitoringWell { get; set; }
        public string Cpt { get; set; }
        public string Note { get; set; }
        public decimal? WoodLevel { get; set; }
        public decimal? GroundwaterLevel { get; set; }
        public decimal? GroundLevel { get; set; }
        public bool FoundationRecoveryAdviced { get; set; }
        public string _FoundationDamageCause { get; set; }
        public short? BuiltYear { get; set; }
        public Guid _Address { get; set; }
        public string _EnforcementTerm { get; set; }
        public string _BaseMeasurementLevel { get; set; }

        [IgnoreDataMember]
        public virtual Address Address { get; set; }

        [IgnoreDataMember]
        public virtual BaseLevel BaseMeasurementLevel { get; set; }

        [IgnoreDataMember]
        public virtual EnforcementTerm EnforcementTerm { get; set; }

        [IgnoreDataMember]
        public virtual FoundationDamageCause FoundationDamageCause { get; set; }

        [IgnoreDataMember]
        public virtual FoundationQuality FoundationQuality { get; set; }

        [IgnoreDataMember]
        public virtual FoundationType FoundationType { get; set; }

        [IgnoreDataMember]
        public virtual Report ReportNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Substructure Substructure { get; set; }
    }
}
