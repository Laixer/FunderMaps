using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects
{
    /// <summary>
    ///     User DTO.
    /// </summary>
    public record UserDto
    {
        /// <summary>
        ///     Unique identifier.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     User firstname.
        /// </summary>
        public string GivenName { get; init; }

        /// <summary>
        ///     User lastname.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        ///     Unique email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; init; }

        /// <summary>
        ///     Avatar.
        /// </summary>
        public string Avatar { get; init; }

        /// <summary>
        ///     Job title.
        /// </summary>
        public string JobTitle { get; init; }

        /// <summary>
        ///     Phone number.
        /// </summary>
        [Phone]
        public string PhoneNumber { get; init; }

        /// <summary>
        ///     User role.
        /// </summary>
        [Required]
        public ApplicationRole Role { get; init; } = ApplicationRole.User;
    }
}
