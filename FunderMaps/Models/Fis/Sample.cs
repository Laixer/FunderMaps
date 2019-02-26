using System;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class Sample : AccessControl, ISoftDeletable
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
