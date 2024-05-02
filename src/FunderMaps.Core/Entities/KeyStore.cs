using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Key store entity.
/// </summary>
public sealed class KeyStore : IEntityIdentifier<string>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public string Id => Name;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the name for the contractor.
    /// </summary>
    [Required]
    public string Value { get; set; } = default!;

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing contractor.</returns>
    public override string ToString() => Name;
}
