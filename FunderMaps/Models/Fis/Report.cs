using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public partial class Report : AccessControl
    {
        public Report()
        {
            Sample = new HashSet<Sample>();
        }

        public int Id { get; set; }
        public int? Project { get; set; }
        public string DocumentId { get; set; }
        public bool Inspection { get; set; }
        public bool JointMeasurement { get; set; }
        public bool FloorMeasurement { get; set; }
        public bool ConformF3o { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        [IgnoreDataMember]
        public int? Reviewer { get; set; }

        [IgnoreDataMember]
        public int? Creator { get; set; }

        [IgnoreDataMember]
        public int Owner { get; set; }

        [IgnoreDataMember]
        public int Contractor { get; set; }

        public DateTime DocumentDate { get; set; }
        public string DocumentName { get; set; }

        public Organization ContractorNavigation { get; set; }
        public Principal CreatorNavigation { get; set; }
        public Organization OwnerNavigation { get; set; }
        public Project ProjectNavigation { get; set; }
        public Principal ReviewerNavigation { get; set; }
        public ReportStatus StatusNavigation { get; set; }
        public ReportType TypeNavigation { get; set; }
        public ICollection<Sample> Sample { get; set; }
    }
}
