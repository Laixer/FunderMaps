using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Contractor entity.
/// </summary>
public sealed class Contractor
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the name for the contractor.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing contractor.</returns>
    public override string ToString() => Name;
}
