using System.ComponentModel.DataAnnotations;
using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Bundle entity.
/// </summary>
public sealed class ProductCall
{
    /// <summary>
    ///     Organization identifier.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the name for the bundle.
    /// </summary>
    [Required]
    public string Product { get; set; } = default!;

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Geocoder]
    public string? BuildingId { get; set; }

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;

    /// <summary>
    ///     Record create date.
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    ///     Get the precondition for the bundle.
    /// </summary>
    [Required]
    public string Request { get; set; } = default!;
}
