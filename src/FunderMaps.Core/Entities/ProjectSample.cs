using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Project sample entity.
    /// </summary>
    public sealed class ProjectSample : RecordControl
    {
        /// <summary>
        ///     Project sample identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Project identifier.
        /// </summary>
        public int Project { get; set; }

        /// <summary>
        ///     Status.
        /// </summary>
        public ProjectSampleStatus Status { get; set; }

        /// <summary>
        ///     Contact email.
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        ///     Address identifier.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        ///     Project object.
        /// </summary>
        public Project ProjectNavigation { get; set; }

        /// <summary>
        ///     Contact object.
        /// </summary>
        public Contact ContactNavigation { get; set; }

        /// <summary>
        ///     Address object.
        /// </summary>
        public Address AddressNavigation { get; set; }
    }
}
