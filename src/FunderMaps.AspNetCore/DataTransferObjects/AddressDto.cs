using FunderMaps.Core.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Address DTO.
/// </summary>
public sealed record AddressDto
{
    /// <summary>
    ///     Address identifier.
    /// </summary>
    [Geocoder]
    public string Id { get; init; }

    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Geocoder]
    public string BuildingId { get; init; }

    /// <summary>
    ///     Building number.
    /// </summary>
    [Required]
    public string BuildingNumber { get; init; }

    /// <summary>
    ///     Postcode.
    /// </summary>
    public string PostalCode { get; init; }

    /// <summary>
    ///     Street name.
    /// </summary>
    [Required]
    public string Street { get; init; }

    /// <summary>
    ///     City.
    /// </summary>
    [Required]
    public string City { get; init; }
}
