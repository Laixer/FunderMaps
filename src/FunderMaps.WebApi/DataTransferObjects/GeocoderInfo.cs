using FunderMaps.Core.Entities;

namespace FunderMaps.WebApi.DataTransferObjects;

/// <summary>
///     Geocoder information.
/// </summary>
public class GeocoderInfo
{
    /// <summary>
    ///    Building information.
    /// </summary>
    public required Building Building { get; set; }

    /// <summary>
    ///     Address information.
    /// </summary>
    public required Address Address { get; set; }

    /// <summary>
    ///    Residence information.
    /// </summary>
    public required Residence Residence { get; set; }

    /// <summary>
    ///     Neighborhood information.
    /// </summary>
    public Neighborhood? Neighborhood { get; set; }

    /// <summary>
    ///     District information.
    /// </summary>
    public District? District { get; set; }

    /// <summary>
    ///    Municipality information.
    /// </summary>
    public Municipality? Municipality { get; set; }

    /// <summary>
    ///    State information.
    /// </summary>
    public State? State { get; set; }
}
