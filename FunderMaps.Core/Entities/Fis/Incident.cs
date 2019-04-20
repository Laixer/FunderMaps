using System;

namespace FunderMaps.Core.Entities.Fis
{
    public class Incident : BaseEntity
    {
        public int Id { get; set; }
        public string FoundationType { get; set; }
        public string FoundationQuality { get; set; }
        public string Substructure { get; set; }
        public string Note { get; set; }
        public string FoundationDamageCause { get; set; }
        public Guid Address { get; set; }
        public int Owner { get; set; }
        public string DocumentName { get; set; }

        public virtual Address AddressNavigation { get; set; }
        public virtual FoundationDamageCause FoundationDamageCauseNavigation { get; set; }
        public virtual FoundationQuality FoundationQualityNavigation { get; set; }
        public virtual FoundationType FoundationTypeNavigation { get; set; }
        public virtual Principal OwnerNavigation { get; set; }
        public virtual Substructure SubstructureNavigation { get; set; }
    }
}
