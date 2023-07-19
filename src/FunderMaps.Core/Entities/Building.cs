using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Building entity.
/// </summary>
public sealed class Building : IEntityIdentifier<string>
{
    /// <summary>
    ///     Entity identifier.
    /// </summary>
    public string Identifier => Id;

    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required, Geocoder]
    public string Id { get; set; } = default!;

    /// <summary>
    ///     Building built year.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime? BuiltYear { get; set; }

    /// <summary>
    ///     Building is active or not.
    /// </summary>
    [Required]
    public bool IsActive { get; set; } = true;

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;

    // TODO: Add building type

    /// <summary>
    ///     Neighborhood identifier.
    /// </summary>
    [Geocoder]
    public string? NeighborhoodId { get; set; }
}
