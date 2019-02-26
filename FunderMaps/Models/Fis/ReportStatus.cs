using System;
using System.Collections.Generic;

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

        public ICollection<Report> Report { get; set; }
    }
}
