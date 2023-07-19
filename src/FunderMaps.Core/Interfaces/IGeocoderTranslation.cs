using FunderMaps.Core.Entities;

namespace FunderMaps.Core.Interfaces;

/// <summary>
///     Translate any geocoder identifier to an internal entity.
/// </summary>
public interface IGeocoderTranslation
{
    /// <summary>
    ///     Convert geocoder identifier to address entity.
    /// </summary>
    /// <param name="input">Input identifier.</param>
    /// <returns>If found returns the <see cref="Address"/> entity.</returns>
    Task<Address> GetAddressIdAsync(string input);

    Task<Building> GetBuildingIdAsync(string input);
}
