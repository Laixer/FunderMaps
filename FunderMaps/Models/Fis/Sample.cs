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

        [Required]
        [BindProperty(Name = "street_name")]
        public string StreetName { get; set; }

        [Required]
        [BindProperty(Name = "building_number")]
        public short? BuildingNumber { get; set; }

        [BindProperty(Name = "building_number_suffix")]
        public string BuildingNumberSuffix { get; set; }

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

        [BindProperty(Name = "groud_level")]
        public decimal? GroudLevel { get; set; }

        [BindProperty(Name = "foundation_recovery_adviced")]
        public bool FoundationRecoveryAdviced { get; set; }

        [BindProperty(Name = "foundation_damage_cause")]
        public string FoundationDamageCause { get; set; }

        [BindProperty(Name = "built_year")]
        public short? BuiltYear { get; set; }

        public Address Address { get; set; }

        [IgnoreDataMember]
        public FoundationDamageCause FoundationDamageCauseNavigation { get; set; }

        [IgnoreDataMember]
        public FoundationQuality FoundationQualityNavigation { get; set; }

        [IgnoreDataMember]
        public FoundationType FoundationTypeNavigation { get; set; }

        [IgnoreDataMember]
        public Report ReportNavigation { get; set; }

        [IgnoreDataMember]
        public Substructure SubstructureNavigation { get; set; }
    }
}
