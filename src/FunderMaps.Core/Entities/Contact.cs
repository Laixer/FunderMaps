using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Contact information.
/// </summary>
public sealed class Contact : IdentifiableEntity<Contact, string>
{
    /// <summary>
    ///     Create new instance.
    /// </summary>
    public Contact()
        : base(e => e.Email)
    {
    }

    /// <summary>
    ///     Contact email.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; }

    /// <summary>
    ///     Contact name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Contact phone number.
    /// </summary>
    [Phone]
    [StringLength(16)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing contact.</returns>
    public override string ToString() => Email;

    /// <summary>
    ///     Check if self is equal to other entity.
    /// </summary>
    /// <param name="other">Entity to compare.</param>
    /// <returns><c>True</c> on success, false otherwise.</returns>
    public override bool Equals(Contact other)
        => other is not null && Email == other.Email;
}
