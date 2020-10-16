using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     User DTO.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        ///     User firstname.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        ///     User lastname.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Unique email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        ///     Job title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        ///     Phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     User role.
        /// </summary>
        [Required]
        public ApplicationRole Role { get; set; } = ApplicationRole.User;
    }
}
