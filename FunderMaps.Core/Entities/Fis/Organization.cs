﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities.Fis
{
    public class Organization : BaseEntity
    {
        public Organization()
        {
            AttributionContractorNavigation = new HashSet<Attribution>();
            AttributionOwnerNavigation = new HashSet<Attribution>();
            Principal = new HashSet<Principal>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Attribution> AttributionContractorNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Attribution> AttributionOwnerNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Principal> Principal { get; set; }
    }
}
