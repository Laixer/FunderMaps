﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Report entity.
    /// </summary>
    public class Report : AttributionControl
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Client document identifier.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string DocumentId { get; set; }

        /// <summary>
        /// Inspection.
        /// </summary>
        public bool Inspection { get; set; }

        /// <summary>
        /// Joint measurement.
        /// </summary>
        public bool JointMeasurement { get; set; }

        /// <summary>
        /// Floor measurement.
        /// </summary>
        public bool FloorMeasurement { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        /// <summary>
        /// Report status.
        /// </summary>
        public ReportStatus Status { get; set; }

        /// <summary>
        /// Report type.
        /// </summary>
        public ReportType Type { get; set; }

        /// <summary>
        /// Original document creation.
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "01/01/1000", "01/01/2100")]
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// Document URL.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string DocumentName { get; set; }

        /// <summary>
        /// List of norms.
        /// </summary>
        public Norm[] Norm { get; set; }
    }
}
