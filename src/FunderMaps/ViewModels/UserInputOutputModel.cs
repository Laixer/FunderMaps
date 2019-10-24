using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.ViewModels
{
    // TODO: Can be removed?
    /// <summary>
    /// User profile model.
    /// </summary>
    public sealed class ProfileInputOutputModel
    {
        /// <summary>
        /// User Identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the given name for the user.
        /// </summary>
        [MaxLength(256)]
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the last name for the user.
        /// </summary>
        [MaxLength(256)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a user avatar.
        /// </summary>
        [MaxLength(256)]
        public string Avatar { get; set; }

        /// <summary>
        /// Gets or sets the job title for the user.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the phone number for the user.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
