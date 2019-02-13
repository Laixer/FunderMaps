using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public partial class FoundationRecovery
    {
        public string EvidenceDocument { get; set; }
        public string Note { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }
        public DateTimeOffset? DeleteDate { get; set; }
        public string EvidenceName { get; set; }
        public string ContractorName { get; set; }
        public string Evidence { get; set; }
        public string Type { get; set; }
        public string StreetName { get; set; }
        public short BuildingNumber { get; set; }
        public string BuildingNumberSuffix { get; set; }
        public short Year { get; set; }

        public Address Address { get; set; }
        public FoundationRecoveryEvidence EvidenceNavigation { get; set; }
        public FoundationRecoveryType TypeNavigation { get; set; }
    }
}
