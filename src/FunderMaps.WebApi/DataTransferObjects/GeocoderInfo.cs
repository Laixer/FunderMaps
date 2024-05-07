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
    public Building Building { get; set; } = default!;

    /// <summary>
    ///     Address information.
    /// </summary>
    public Address Address { get; set; } = default!;

    /// <summary>
    ///    Residence information.
    /// </summary>
    public Residence Residence { get; set; } = default!;

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
