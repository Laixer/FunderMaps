using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class Attribution
    {
        public Attribution()
        {
            FoundationRecovery = new HashSet<FoundationRecovery>();
            Report = new HashSet<Report>();
        }

        public int Id { get; set; }
        public int? Project { get; set; }
        public int? Reviewer { get; set; }

        [Required]
        public int Creator { get; set; }
        public int Owner { get; set; }
        public int Contractor { get; set; }

        public virtual Organization ContractorNavigation { get; set; }
        public virtual Principal CreatorNavigation { get; set; }
        public virtual Organization OwnerNavigation { get; set; }
        public virtual Project ProjectNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Report> Report { get; set; }
    }
}
