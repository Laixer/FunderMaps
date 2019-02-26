using System;
using System.Collections.Generic;

namespace FunderMaps.Models.Fis
{
    public class Principal
    {
        public Principal()
        {
            ProjectAdviserNavigation = new HashSet<Project>();
            ProjectCreatorNavigation = new HashSet<Project>();
            ProjectLeadNavigation = new HashSet<Project>();
            ReportCreatorNavigation = new HashSet<Report>();
            ReportReviewerNavigation = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string NickName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Organization { get; set; }

        public Organization OrganizationNavigation { get; set; }
        public ICollection<Project> ProjectAdviserNavigation { get; set; }
        public ICollection<Project> ProjectCreatorNavigation { get; set; }
        public ICollection<Project> ProjectLeadNavigation { get; set; }
        public ICollection<Report> ReportCreatorNavigation { get; set; }
        public ICollection<Report> ReportReviewerNavigation { get; set; }
    }
}
