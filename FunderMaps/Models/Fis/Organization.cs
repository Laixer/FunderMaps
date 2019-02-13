using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public partial class Organization
    {
        public Organization()
        {
            Principal = new HashSet<Principal>();
            ReportContractorNavigation = new HashSet<Report>();
            ReportOwnerNavigation = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Principal> Principal { get; set; }
        public ICollection<Report> ReportContractorNavigation { get; set; }
        public ICollection<Report> ReportOwnerNavigation { get; set; }
    }
}
