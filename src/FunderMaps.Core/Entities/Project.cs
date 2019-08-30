using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Project entity.
    /// </summary>
    public class Project : RecordControl
    {
        /// <summary>
        /// Project identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Dossier name.
        /// </summary>
        [MaxLength(256)]
        public string Dossier { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        [IgnoreDataMember]
        public int? Adviser { get; set; }

        [IgnoreDataMember]
        public int? Lead { get; set; }

        [IgnoreDataMember]
        public int? Creator { get; set; }

        public Principal AdviserNavigation { get; set; }

        public Principal CreatorNavigation { get; set; }

        public Principal LeadNavigation { get; set; }
    }
}
