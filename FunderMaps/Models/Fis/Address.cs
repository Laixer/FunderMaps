using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Incident> Incident { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Sample> Sample { get; set; }
    }
}
