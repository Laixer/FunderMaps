using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Components;

/// <summary>
///     Translate any geocoder identifier to an internal entity.
/// </summary>
/// <remarks>
///     Create new instance.
/// </remarks>
internal class GeocoderTranslation(
    IGeocoderParser geocoderParser,
    IAddressRepository addressRepository,
    IBuildingRepository buildingRepository) : IGeocoderTranslation
{
    /// <summary>
    ///     Convert geocoder identifier to address entity.
    /// </summary>
    /// <param name="input">Input identifier.</param>
    /// <returns>If found returns the <see cref="Address"/> entity.</returns>
    public async Task<Address> GetAddressIdAsync(string input)
        => geocoderParser.FromIdentifier(input, out string id) switch
        {
            GeocoderDatasource.FunderMaps => await addressRepository.GetByIdAsync(id),
            GeocoderDatasource.NlBagAddress => await addressRepository.GetByExternalIdAsync(id),
            _ => throw new EntityNotFoundException("Requested address entity could not be found."),
        };

    /// <summary>
    ///     Convert geocoder identifier to building entity.
    /// </summary>
    /// <param name="input">Input identifier.</param>
    /// <returns>If found returns the <see cref="Building"/> entity.</returns>
    public async Task<Building> GetBuildingIdAsync(string input)
        => geocoderParser.FromIdentifier(input, out string id) switch
        {
            GeocoderDatasource.FunderMaps => await buildingRepository.GetByIdAsync(id),
            GeocoderDatasource.NlBagAddress => await buildingRepository.GetByExternalAddressIdAsync(id),
            GeocoderDatasource.NlBagBuilding => await buildingRepository.GetByExternalIdAsync(id),
            _ => throw new EntityNotFoundException("Requested building entity could not be found."),
        };
}
