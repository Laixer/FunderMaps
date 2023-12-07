using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Email;

/// <summary>
///     Email address.
/// </summary>
public record EmailAddress
{
    /// <summary>
    ///    Create new instance.
    /// </summary>
    /// <param name="address">Address.</param>
    /// <param name="name">Name.</param>
    public EmailAddress(string address, string? name = null)
    {
        Address = address;
        Name = name ?? address;
    }

    /// <summary>
    ///     Name corresponding to address.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Email address.
    /// </summary>
    [Required, EmailAddress]
    public string Address { get; init; }

    /// <summary>
    ///     Print address as string.
    /// </summary>
    public override string? ToString() => Address;
}
