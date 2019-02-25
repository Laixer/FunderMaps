using System;

namespace FunderMaps.Models.Fis
{
    public partial class Sample : AccessControl
    {
        public int Id { get; set; }
        public int Report { get; set; }
        public string StreetName { get; set; }
        public short BuildingNumber { get; set; }
        public string BuildingNumberSuffix { get; set; }
        public string FoundationType { get; set; }
        public string FoundationQuality { get; set; }
        public string Substructure { get; set; }
        public string MonitoringWell { get; set; }
        public string Cpt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Note { get; set; }
        public decimal? WoodLevel { get; set; }
        public decimal? GroundwaterLevel { get; set; }
        public decimal? Groudlevel { get; set; }
        public bool FoundationRecoveryAdviced { get; set; }
        public string FoundationDamageCause { get; set; }
        public short? BuiltYear { get; set; }

        public Address Address { get; set; }
        public FoundationDamageCause FoundationDamageCauseNavigation { get; set; }
        public FoundationQuality FoundationQualityNavigation { get; set; }
        public FoundationType FoundationTypeNavigation { get; set; }
        public Report ReportNavigation { get; set; }
        public Substructure SubstructureNavigation { get; set; }
    }
}
