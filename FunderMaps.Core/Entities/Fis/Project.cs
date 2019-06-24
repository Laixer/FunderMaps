using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities.Fis
{
    /// <summary>
    /// Project entity.
    /// </summary>
    public class Project : RecordControl
    {
        public Project()
        {
            Attribution = new HashSet<Attribution>();
        }

        public int Id { get; set; }
        [MaxLength(256)]
        public string Dossier { get; set; }
        public string Note { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Adviser { get; set; }
        public int? Lead { get; set; }
        public int? Creator { get; set; }

        public virtual Principal AdviserNavigation { get; set; }
        public virtual Principal CreatorNavigation { get; set; }
        public virtual Principal LeadNavigation { get; set; }
        public virtual ICollection<Attribution> Attribution { get; set; }
    }
}
