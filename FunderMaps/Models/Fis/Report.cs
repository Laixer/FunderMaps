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

        [Required]
        [BindProperty(Name = "document_id")]
        public string DocumentId { get; set; }

        [BindProperty(Name = "inspection")]
        public bool Inspection { get; set; }

        [BindProperty(Name = "joint_measurement")]
        public bool JointMeasurement { get; set; }

        [BindProperty(Name = "floor_measurement")]
        public bool FloorMeasurement { get; set; }

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

        [Required]
        [BindProperty(Name = "document_date")]
        public DateTime DocumentDate { get; set; }

        [BindProperty(Name = "document_name")]
        public string DocumentName { get; set; }

        [BindProperty(Name = "attribution")]
        public int Attribution { get; set; }

        [BindProperty(Name = "access_policy_navigation")]
        public virtual AccessPolicy AccessPolicyNavigation { get; set; }

        [BindProperty(Name = "attribution_navigation")]
        public virtual Attribution AttributionNavigation { get; set; }

        [BindProperty(Name = "status_navigation")]
        public virtual ReportStatus StatusNavigation { get; set; }

        [BindProperty(Name = "type_navigation")]
        public virtual ReportType TypeNavigation { get; set; }

        [BindProperty(Name = "norm")]
        public virtual Norm Norm { get; set; }

        [IgnoreDataMember]
        public virtual ICollection<Sample> Sample { get; set; }

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
