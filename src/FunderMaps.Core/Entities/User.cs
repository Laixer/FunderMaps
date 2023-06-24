using FunderMaps.Core.Identity;
using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     User entity.
/// </summary>
public sealed class User : IdentifiableEntity<User, Guid>, IUser
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public User()
        : base(e => e.Id)
    {
    }

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     User firstname.
    /// </summary>
    public string GivenName { get; set; }

    // TODO: Surname
    /// <summary>
    ///     User lastname.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    ///     Unique email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; }

    // TODO: Remove
    /// <summary>
    ///     Avatar.
    /// </summary>
    public string Avatar { get; set; }

    // TODO: Remove
    /// <summary>
    ///     Job title.
    /// </summary>
    public string JobTitle { get; set; }

    /// <summary>
    ///     Phone number.
    /// </summary>
    [Phone]
    public string PhoneNumber { get; set; }

    // TODO: Move up
    /// <summary>
    ///     User role.
    /// </summary>
    [Required]
    public ApplicationRole Role { get; set; } = ApplicationRole.User;

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing user.</returns>
    public override string ToString()
        => !string.IsNullOrEmpty(GivenName)
            ? (!string.IsNullOrEmpty(LastName) ? $"{GivenName} {LastName}" : GivenName)
            : (!string.IsNullOrEmpty(Email) ? Email : Id.ToString());

    /// <summary>
    ///     Check if self is equal to other entity.
    /// </summary>
    /// <param name="other">Entity to compare.</param>
    /// <returns><c>True</c> on success, false otherwise.</returns>
    public override bool Equals(User other) => Id == other.Id;
}
