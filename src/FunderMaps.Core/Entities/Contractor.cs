using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Contractor entity.
/// </summary>
public sealed class Contractor : IEntityIdentifier<int>
{
    public int Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the name for the contractor.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    ///     Print object as name.
    /// </summary>
    /// <returns>String representing contractor.</returns>
    public override string ToString() => Name;
}
