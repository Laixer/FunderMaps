using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.Core.Entities;

/// <summary>
///     Address entity.
/// </summary>
public sealed class Address : IEntityIdentifier<string>
{
    /// <summary>
    ///     Unique identifier.
    /// </summary>
    [Required, Geocoder]
    public string Id { get; set; } = default!;

    /// <summary>
    ///     Building number.
    /// </summary>
    [Required]
    public string BuildingNumber { get; set; } = default!;

    /// <summary>
    ///     Postcode.
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    ///     Street name.
    /// </summary>
    [Required]
    public string Street { get; set; } = default!;

    /// <summary>
    ///     Address is active or not.
    /// </summary>
    [Required]
    public bool IsActive { get; set; } = true;

    /// <summary>
    ///     External data source id.
    /// </summary>
    [Required]
    public string ExternalId { get; set; } = default!;

    /// <summary>
    ///     City.
    /// </summary>
    [Required]
    public string City { get; set; } = default!;

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Geocoder]
    public string? BuildingId { get; set; }

    /// <summary>
    ///     Full address.
    /// </summary>
    public string FullAddress => PostalCode is not null
        ? $"{Street} {BuildingNumber}, {PostalCode} {City}"
        : $"{Street} {BuildingNumber}, {City}";
}
