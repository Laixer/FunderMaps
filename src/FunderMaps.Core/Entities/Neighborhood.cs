using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Neighborhood entity.
/// </summary>
public sealed class Neighborhood : IEntityIdentifier<string>
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
    ///     Neighborhood name.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Geocoder]
    public string? DistrictId { get; set; }
}
