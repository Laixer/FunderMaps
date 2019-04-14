using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class Address
    {
        public Address()
        {
            FoundationRecovery = new HashSet<FoundationRecovery>();
            Incident = new HashSet<Incident>();
            Sample = new HashSet<Sample>();
        }

        public Guid Id { get; set; }
        public string StreetName { get; set; }
        public short BuildingNumber { get; set; }
        public string BuildingNumberSuffix { get; set; }

        public virtual Object Object { get; set; }
        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }
        public virtual ICollection<Incident> Incident { get; set; }
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
