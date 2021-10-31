using FunderMaps.Core.DataAnnotations;

namespace FunderMaps.AspNetCore.DataTransferObjects;

/// <summary>
///     Base class for analysis endpoint responses.
/// </summary>
public record AnalysisDto
{
    /// <summary>
    ///     Building identifier.
    /// </summary>
    [Geocoder]
    public string Id { get; set; }

    /// <summary>
    ///     Building external identifier.
    /// </summary>
    public string ExternalId { get; set; }

    /// <summary>
    ///     Postcode.
    /// </summary>
    public string PostalCode { get; set; }

    /// <summary>
    ///     Address identifier.
    /// </summary>
    [Geocoder]
    public string AddressId { get; set; }

    /// <summary>
    ///     Address external identifier.
    /// </summary>
    public string AddressExternalId { get; set; }

    /// <summary>
    ///     Neighborhood identifier.
    /// </summary>
    [Geocoder]
    public string NeighborhoodId { get; set; }
}
