using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Municipality entity.
/// </summary>
public sealed class Municipality : IEntityIdentifier<string>
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
    ///     Municipality name.
    /// </summary>
    [Required]
    public string Name { get; set; } = default!;

    /// <summary>
    ///    Is water municipality.
    /// </summary>
    public bool Water { get; set; }

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;

    /// <summary>
    ///     State identifier.
    /// </summary>
    [Geocoder]
    public string? StateId { get; set; }
}
