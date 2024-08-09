using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     District entity.
/// </summary>
public sealed class District
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required, Geocoder]
    public string Id { get; set; } = default!;

    /// <summary>
    ///     District name.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///    Is water district.
    /// </summary>
    public bool Water { get; set; }

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;

    /// <summary>
    ///     Municipality identifier.
    /// </summary>
    [Geocoder]
    public string? MunicipalityId { get; set; }
}
