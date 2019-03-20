using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.Models.Fis
{
    public class Report : AccessControl, ISoftDeletable
    {
        public Report()
        {
            Sample = new HashSet<Sample>();
        }

        [BindProperty(Name = "id")]
        public int Id { get; set; }

        [BindProperty(Name = "project")]
        public int? Project { get; set; }

        [Required]
        [BindProperty(Name = "document_id")]
        public string DocumentId { get; set; }

        [BindProperty(Name = "inspection")]
        public bool Inspection { get; set; }

        [BindProperty(Name = "joint_measurement")]
        public bool JointMeasurement { get; set; }

        [BindProperty(Name = "floor_measurement")]
        public bool FloorMeasurement { get; set; }

        [BindProperty(Name = "conform_f3o")]
        public bool ConformF3o { get; set; }

        [BindProperty(Name = "create_date")]
        public DateTime CreateDate { get; set; }

        [BindProperty(Name = "update_date")]
        public DateTime? UpdateDate { get; set; }

        [BindProperty(Name = "delete_date")]
        public DateTime? DeleteDate { get; set; }

        [BindProperty(Name = "note")]
        public string Note { get; set; }

        [BindProperty(Name = "status")]
        public string Status { get; set; }

        [BindProperty(Name = "type")]
        public string Type { get; set; }

        [BindProperty(Name = "reviewer")]
        public int? Reviewer { get; set; }

        [IgnoreDataMember]
        public int? Creator { get; set; }

        [IgnoreDataMember]
        public int Owner { get; set; }

        [Required]
        [BindProperty(Name = "contractor")]
        public int Contractor { get; set; }

        [Required]
        [BindProperty(Name = "document_date")]
        public DateTime DocumentDate { get; set; }

        [BindProperty(Name = "document_name")]
        public string DocumentName { get; set; }

        [IgnoreDataMember]
        public Organization ContractorNavigation { get; set; }

        [IgnoreDataMember]
        public Principal CreatorNavigation { get; set; }

        [IgnoreDataMember]
        public Organization OwnerNavigation { get; set; }

        [IgnoreDataMember]
        public Project ProjectNavigation { get; set; }

        [IgnoreDataMember]
        public Principal ReviewerNavigation { get; set; }

        [BindProperty(Name = "status_navigation")]
        public ReportStatus StatusNavigation { get; set; }

        [BindProperty(Name = "type_navigation")]
        public ReportType TypeNavigation { get; set; }

        [IgnoreDataMember]
        public ICollection<Sample> Sample { get; set; }

        /// <summary>
        /// Check report status to see if new samples
        /// can be added to the report.
        /// </summary>
        /// <returns>True on success.</returns>
        public bool CanHaveNewSamples()
        {
            switch (Status)
            {
                case "todo":
                case "pending":
                    return true;
            }

            return false;
        }
    }
}
