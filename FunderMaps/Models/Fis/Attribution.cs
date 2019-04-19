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

        [IgnoreDataMember]
        public int? _Reviewer { get; set; }

        [IgnoreDataMember]
        public int _Contractor { get; set; }

        [IgnoreDataMember]
        public int _Creator { get; set; }

        [IgnoreDataMember]
        public int _Owner { get; set; }

        public virtual Principal Reviewer { get; set; }

        [Required]
        public virtual Organization Contractor { get; set; }

        public virtual Principal Creator { get; set; }
        public virtual Organization Owner { get; set; }
        public virtual Project ProjectNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<FoundationRecovery> FoundationRecovery { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Report> Report { get; set; }
    }
}
