﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Indicent entity.
    /// </summary>
    public class Incident : BaseEntity
    {
        public int Id { get; set; }
        public FoundationType FoundationType { get; set; }
        public FoundationQuality? FoundationQuality { get; set; }
        public Substructure Substructure { get; set; }
        public string Note { get; set; }
        public FoundationDamageCause FoundationDamageCause { get; set; }
        public Guid Address { get; set; }
        public int Owner { get; set; }
        [MaxLength(256)]
        public string DocumentName { get; set; }

        public virtual Address AddressNavigation { get; set; }
        public virtual FoundationQuality FoundationQualityNavigation { get; set; }
        public virtual Principal OwnerNavigation { get; set; }
    }
}
