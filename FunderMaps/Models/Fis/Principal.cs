using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class Principal
    {
        public Principal()
        {
            Attribution = new HashSet<Attribution>();
            Incident = new HashSet<Incident>();
            ProjectAdviserNavigation = new HashSet<Project>();
            ProjectCreatorNavigation = new HashSet<Project>();
            ProjectLeadNavigation = new HashSet<Project>();
        }

        public int Id { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Organization { get; set; }
        public string Phone { get; set; }

        public virtual Organization OrganizationNavigation { get; set; }
        public virtual ICollection<Attribution> Attribution { get; set; }
        public virtual ICollection<Incident> Incident { get; set; }
        public virtual ICollection<Project> ProjectAdviserNavigation { get; set; }
        public virtual ICollection<Project> ProjectCreatorNavigation { get; set; }
        public virtual ICollection<Project> ProjectLeadNavigation { get; set; }
    }
}
