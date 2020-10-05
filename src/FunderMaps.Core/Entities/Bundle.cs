using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Represents a bundle of layers made by an organization.
    /// </summary>
    public sealed class Bundle : IdentifiableEntity<Bundle, Guid>
    {
        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Bundle()
            : base(e => e.Id)
        {
        }

        /// <summary>
        ///     Unique identifier.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        ///     References the organization which owns this bundle.
        /// </summary>
        [Required]
        public Guid OrganizationId { get; set; }

        /// <summary>
        ///     References the user that created this bundle.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        ///     Unique identifier for this bundle version.
        /// </summary>
        [Required]
        public Guid VersionId { get; set; }

        /// <summary>
        ///     Name of this bundle.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     The date this bundle was created.
        /// </summary>
        [Required]
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        ///     The date this bundle was last updated.
        /// </summary>
        public DateTimeOffset? UpdateDate { get; set; }

        /// <summary>
        ///     The date this bundle was deleted.
        /// </summary>
        public DateTimeOffset? DeleteDate { get; set; }

        /// <summary>
        ///     All ids of layers belonging to this bundle.
        /// </summary>
        [Required]
        public IEnumerable<Guid> LayersIds { get; set; }
    }
}
