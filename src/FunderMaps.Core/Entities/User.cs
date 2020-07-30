using FunderMaps.Core.Types;
using System;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     User entity.
    /// </summary>
    public sealed class User : BaseEntity
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
        /// Unique email address.
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

        /// <summary>
        ///     Print object as name.
        /// </summary>
        /// <returns>String representing user.</returns>
        public override string ToString() => Email;

        /// <summary>
        ///     Initialize property defaults.
        /// </summary>
        public void InitializeDefaults()
        {
            Id = Guid.Empty;
            Role = ApplicationRole.User;
        }

        /// <summary>
        ///     Initialize properties from another entity.
        /// </summary>
        public void InitializeDefaults(User other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            Id = other.Id;
            Email = other.Email;
            Role = other.Role;
        }

        public override void Validate()
        {
            base.Validate();

            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
