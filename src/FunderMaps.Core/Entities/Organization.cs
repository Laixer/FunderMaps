using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Organization entity.
/// </summary>
public sealed class Organization : IEntityIdentifier<Guid>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the name for the organization.
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing organization.</returns>
    public override string ToString() => Name;
}
