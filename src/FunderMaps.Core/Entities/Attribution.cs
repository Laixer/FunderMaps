using System;
using System.Runtime.Serialization;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Attribution represents a entity partition for user and organizational relations.
    /// </summary>
    public abstract class Attribution
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Project relation identifier or null.
        /// </summary>
        public int? Project { get; set; }

        /// <summary>
        ///     Reviewer idenitfier.
        /// </summary>
        public Guid? Reviewer { get; set; }

        /// <summary>
        ///     Contractor identifier.
        /// </summary>
        public Guid Contractor { get; set; }

        /// <summary>
        ///     Creator identifier.
        /// </summary>
        public Guid Creator { get; set; }

        /// <summary>
        ///     Owner identifier.
        /// </summary>
        public Guid Owner { get; set; }

        /// <summary>
        ///     Project relation or null.
        /// </summary>
        [IgnoreDataMember]
        public Project ProjectNavigation { get; set; }

        /// <summary>
        ///     Reviewer or null.
        /// </summary>
        [IgnoreDataMember]
        public Principal ReviewerNavigation { get; set; }

        /// <summary>
        ///     Contractor.
        /// </summary>
        [IgnoreDataMember]
        public Organization ContractorNavigation { get; set; }

        /// <summary>
        ///     Creator.
        /// </summary>
        [IgnoreDataMember]
        public Principal CreatorNavigation { get; set; }

        /// <summary>
        ///     Owner.
        /// </summary>
        [IgnoreDataMember]
        public Organization OwnerNavigation { get; set; }
    }
}
