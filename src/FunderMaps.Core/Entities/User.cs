using FunderMaps.Core.Types;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     User entity.
/// </summary>
public class User : IEntityIdentifier<Guid>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     User firstname.
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    ///     User lastname.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    ///     Unique email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    /// <summary>
    ///     Avatar.
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    ///     Job title.
    /// </summary>
    public string? JobTitle { get; set; }

    /// <summary>
    ///     Phone number.
    /// </summary>
    [Phone]
    public string? PhoneNumber { get; set; }

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
}
