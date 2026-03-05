using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Building entity.
/// </summary>
public sealed class Building : IEntityIdentifier<string>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required, Geocoder]
    public required string Id { get; set; }

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
    public required string ExternalId { get; set; }

    // TODO: Add building type

    /// <summary>
    ///     Neighborhood identifier.
    /// </summary>
    [Geocoder]
    public string? NeighborhoodId { get; set; }
}
