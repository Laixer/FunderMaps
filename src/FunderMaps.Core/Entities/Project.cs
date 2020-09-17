using FunderMaps.Core.Entities.Report;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    // TODO inherit from StateControl?
    /// <summary>
    ///     Project entity.
    /// </summary>
    public sealed class Project : RecordControl<Project, int>, IReportEntity<Project>
    {
        /// <summary>
        ///     Creaew new instance.
        /// </summary>
        public Project()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Project identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Dossier name.
        /// </summary>
        public string Dossier { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }

        /// <summary>
        ///     Start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        ///     End date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        ///     Adviser user identifier.
        /// </summary>
        public Guid? Adviser { get; set; }

        /// <summary>
        ///     Lead user identifier.
        /// </summary>
        public Guid? Lead { get; set; }

        /// <summary>
        ///     Creator user identifier.
        /// </summary>
        public Guid? Creator { get; set; }

        /// <summary>
        ///     Adviser user.
        /// </summary>
        public User AdviserNavigation { get; set; }

        /// <summary>
        ///     Lead user.
        /// </summary>
        public User LeadNavigation { get; set; }

        /// <summary>
        ///     Creator user.
        /// </summary>
        public User CreatorNavigation { get; set; }
    }
}
