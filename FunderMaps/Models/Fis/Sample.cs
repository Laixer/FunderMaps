using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models.Fis
{
    public class Sample : AccessControl, ISoftDeletable
    {
        public int Id { get; set; }

        [Required]
        public int? Report { get; set; }

        public string FoundationType { get; set; }
        public string FoundationQuality { get; set; }
        public string Substructure { get; set; }
        public string MonitoringWell { get; set; }
        public string Cpt { get; set; }
        public string Note { get; set; }
        public decimal? WoodLevel { get; set; }
        public decimal? GroundwaterLevel { get; set; }
        public decimal? GroundLevel { get; set; }
        public bool FoundationRecoveryAdviced { get; set; }
        public string FoundationDamageCause { get; set; }
        public short? BuiltYear { get; set; }
        public Guid Address { get; set; }
        public string EnforcementTerm { get; set; }
        public string BaseMeasurementLevel { get; set; }

        [IgnoreDataMember]
        public virtual Address AddressNavigation { get; set; }

        [IgnoreDataMember]
        public virtual BaseLevel BaseMeasurementLevelNavigation { get; set; }

        [IgnoreDataMember]
        public virtual EnforcementTerm EnforcementTermNavigation { get; set; }

        [IgnoreDataMember]
        public virtual FoundationDamageCause FoundationDamageCauseNavigation { get; set; }

        [IgnoreDataMember]
        public virtual FoundationQuality FoundationQualityNavigation { get; set; }

        [IgnoreDataMember]
        public virtual FoundationType FoundationTypeNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Report ReportNavigation { get; set; }

        [IgnoreDataMember]
        public virtual Substructure SubstructureNavigation { get; set; }
    }
}
