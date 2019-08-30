using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    /// Attribution represents a entity partition for user and organizational
    /// relations.
    /// </summary>
    public class Attribution
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Project relation identifier or null.
        /// </summary>
        [IgnoreDataMember]
        public int? Project { get; set; }

        /// <summary>
        /// Reviewer idenitfier.
        /// </summary>
        [IgnoreDataMember]
        public int? Reviewer { get; set; }

        /// <summary>
        /// Contractor identifier.
        /// </summary>
        [IgnoreDataMember]
        public int Contractor { get; set; }

        /// <summary>
        /// Creator identifier.
        /// </summary>
        [IgnoreDataMember]
        public int Creator { get; set; }

        /// <summary>
        /// Owner identifier.
        /// </summary>
        [IgnoreDataMember]
        public int Owner { get; set; }

        /// <summary>
        /// Project relation or null.
        /// </summary>
        public Project ProjectNavigation { get; set; }

        /// <summary>
        /// Reviewer or null.
        /// </summary>
        public Principal ReviewerNavigation { get; set; }

        /// <summary>
        /// Contractor.
        /// </summary>
        [Required]
        public Organization ContractorNavigation { get; set; }

        /// <summary>
        /// Creator.
        /// </summary>
        [Required]
        public Principal CreatorNavigation { get; set; }

        /// <summary>
        /// Owner.
        /// </summary>
        [Required]
        public Organization OwnerNavigation { get; set; }
    }
}
