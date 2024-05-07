using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Residence entity.
/// </summary>
public sealed class Residence : IEntityIdentifier<string>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required]
    public string Id { get; set; } = default!;

    /// <summary>
    ///     Address identifier.
    /// </summary>
    [Required]
    public string AddressId { get; set; } = default!;

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Required]
    public string BuildingId { get; set; } = default!;

    /// <summary>
    ///    Longitude.
    /// </summary>
    [Required]
    public float Longitude { get; set; }

    /// <summary>
    ///   Latitude.
    /// </summary>
    [Required]
    public float Latitude { get; set; }
}
