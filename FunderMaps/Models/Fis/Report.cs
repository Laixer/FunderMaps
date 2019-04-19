using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Models.Fis
{
    public class Report : AccessControl, ISoftDeletable
    {
        public Report()
        {
            Sample = new HashSet<Sample>();
        }

        public int Id { get; set; }

        [Required]
        public string DocumentId { get; set; }

        public bool Inspection { get; set; }
        public bool JointMeasurement { get; set; }
        public bool FloorMeasurement { get; set; }

        public string Note { get; set; }

        [IgnoreDataMember]
        public string _Status { get; set; }

        [IgnoreDataMember]
        public string _Type { get; set; }

        [Required]
        public DateTime DocumentDate { get; set; }

        public string DocumentName { get; set; }

        [IgnoreDataMember]
        public int _Attribution { get; set; }

        [Required]
        public virtual Attribution Attribution { get; set; }

        public virtual ReportStatus Status { get; set; }
        public virtual ReportType Type { get; set; }
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
            switch (Status.Id)
            {
                case "todo":
                case "pending":
                    return true;
            }

            return false;
        }
    }
}
