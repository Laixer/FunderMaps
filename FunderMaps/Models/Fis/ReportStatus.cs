using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class ReportStatus
    {
        public ReportStatus()
        {
            Report = new HashSet<Report>();
        }

        public string Id { get; set; }
        public string NameNl { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Report> Report { get; set; }
    }
}
