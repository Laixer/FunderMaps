using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class Principal
    {
        public Principal()
        {
            AttributionReviewerNavigation = new HashSet<Attribution>();
            AttributionCreatorNavigation = new HashSet<Attribution>();
            Incident = new HashSet<Incident>();
            ProjectAdviserNavigation = new HashSet<Project>();
            ProjectCreatorNavigation = new HashSet<Project>();
            ProjectLeadNavigation = new HashSet<Project>();
        }

        public int Id { get; set; }

        [Required]
        public string NickName { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [IgnoreDataMember]
        public int? _Organization { get; set; }

        public string Phone { get; set; }

        public virtual Organization Organization { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Attribution> AttributionReviewerNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Attribution> AttributionCreatorNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Incident> Incident { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Project> ProjectAdviserNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Project> ProjectCreatorNavigation { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Project> ProjectLeadNavigation { get; set; }
    }
}
