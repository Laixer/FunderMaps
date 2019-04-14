using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models.Fis
{
    public class Sample : AccessControl, ISoftDeletable
    {
        [BindProperty(Name = "id")]
        public int Id { get; set; }

        [Required]
        [BindProperty(Name = "report")]
        public int? Report { get; set; }

        [BindProperty(Name = "foundation_type")]
        public string FoundationType { get; set; }

        [BindProperty(Name = "foundation_quality")]
        public string FoundationQuality { get; set; }

        [BindProperty(Name = "substructure")]
        public string Substructure { get; set; }

        [BindProperty(Name = "monitoring_well")]
        public string MonitoringWell { get; set; }

        [BindProperty(Name = "cpt")]
        public string Cpt { get; set; }

        [BindProperty(Name = "create_date")]
        public DateTime CreateDate { get; set; }

        [BindProperty(Name = "update_date")]
        public DateTime? UpdateDate { get; set; }

        [BindProperty(Name = "delete_date")]
        public DateTime? DeleteDate { get; set; }

        [BindProperty(Name = "note")]
        public string Note { get; set; }

        [BindProperty(Name = "wood_level")]
        public decimal? WoodLevel { get; set; }

        [BindProperty(Name = "ground_water_level")]
        public decimal? GroundwaterLevel { get; set; }

        [BindProperty(Name = "ground_level")]
        public decimal? GroundLevel { get; set; }

        [BindProperty(Name = "foundation_recovery_adviced")]
        public bool FoundationRecoveryAdviced { get; set; }

        [BindProperty(Name = "foundation_damage_cause")]
        public string FoundationDamageCause { get; set; }

        [BindProperty(Name = "built_year")]
        public short? BuiltYear { get; set; }

        [BindProperty(Name = "address")]
        public Guid Address { get; set; }

        [BindProperty(Name = "enforcement_term")]
        public string EnforcementTerm { get; set; }

        [BindProperty(Name = "base_measurement_level")]
        public string BaseMeasurementLevel { get; set; }

        [IgnoreDataMember]
        public virtual AccessPolicy AccessPolicyNavigation { get; set; }

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
