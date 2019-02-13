using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public partial class Address
    {
        public Address()
        {
            FoundationRecovery = new HashSet<FoundationRecovery>();
            Sample = new HashSet<Sample>();
        }

        public string StreetName { get; set; }
        public short BuildingNumber { get; set; }
        public string BuildingNumberSuffix { get; set; }
        public string Township { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string Subneighborhood { get; set; }
        public string Note { get; set; }

        public ICollection<FoundationRecovery> FoundationRecovery { get; set; }
        public ICollection<Sample> Sample { get; set; }
    }
}
