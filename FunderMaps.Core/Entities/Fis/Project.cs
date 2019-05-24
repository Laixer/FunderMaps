using System;
using System.Collections.Generic;
//using NpgsqlTypes;

namespace FunderMaps.Core.Entities.Fis
{
    public class Project : RecordControl
    {
        public Project()
        {
            Attribution = new HashSet<Attribution>();
        }

        public int Id { get; set; }
        public string Dossier { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        //public NpgsqlPolygon? Outline { get; set; }
        public int? Adviser { get; set; }
        public int? Lead { get; set; }
        public int? Creator { get; set; }

        public virtual Principal AdviserNavigation { get; set; }
        public virtual Principal CreatorNavigation { get; set; }
        public virtual Principal LeadNavigation { get; set; }
        public virtual ICollection<Attribution> Attribution { get; set; }
    }
}
