using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Services;

/// <summary>
///     Translate any geocoder identifier to an internal entity.
/// </summary>
public class GeocoderTranslation(
    IAddressRepository addressRepository,
    IBuildingRepository buildingRepository,
    IResidenceRepository residenceRepository,
    INeighborhoodRepository neighborhoodRepository,
    IDistrictRepository districtRepository,
    IMunicipalityRepository municipalityRepository,
    IStateRepository stateRepository)
{
    /// <summary>
    ///     Identify the geocoder datasource from the input.
    /// </summary>
    /// <param name="input">Input identifier.</param>
    /// <returns>Geocoder datasource via <see cref="GeocoderDatasource"/>.</returns>
    private static GeocoderDatasource FromIdentifier(string input)
        => input switch
        {
            string when input.StartsWith("gfm-", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.FunderMaps,
            string when input.StartsWith("FIR", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.FundermapsIncidentReport,
            string when input.StartsWith("FQR", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.FundermapsInquiryReport, // TODO: InquirySample
            string when input.StartsWith("FRR", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.FundermapsRecoveryReport, // TODO: RecoverySample
            string when input.Length == 6 && char.IsLetter(input[4]) && char.IsLetter(input[4]) => GeocoderDatasource.NlPostcode,
            string when input.Length == ("NL.IMBAG.PAND.".Length + 16) && input.StartsWith("NL.IMBAG.PAND.", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagBuilding,
            string when input.Length == ("NL.IMBAG.LIGPLAATS.".Length + 16) && input.StartsWith("NL.IMBAG.LIGPLAATS.", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagBerth,
            string when input.Length == ("NL.IMBAG.STANDPLAATS.".Length + 16) && input.StartsWith("NL.IMBAG.STANDPLAATS.", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagPosting,
            string when input.Length == ("NL.IMBAG.VERBLIJFSOBJECT.".Length + 16) && input.StartsWith("NL.IMBAG.VERBLIJFSOBJECT.", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagResidence,
            string when input.Length == ("NL.IMBAG.NUMMERAANDUIDING.".Length + 16) && input.StartsWith("NL.IMBAG.NUMMERAANDUIDING.", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagAddress,
            string when input.Length == 10 && input.StartsWith("BU", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlCbsNeighborhood,
            string when input.Length == 8 && input.StartsWith("WK", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlCbsDistrict,
            string when input.Length == 6 && input.StartsWith("GM", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlCbsMunicipality,
            string when input.Length == 4 && input.StartsWith("PV", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlCbsState,
            string when input.Length == 16 && input.Substring(4, 2) == "10" => GeocoderDatasource.NlBagLegacyBuilding,
            string when input.Length == 16 && input.Substring(4, 2) == "20" => GeocoderDatasource.NlBagLegacyAddress,
            string when input.Length == 16 && input.Substring(4, 2) == "02" => GeocoderDatasource.NlBagLegacyBerth,
            string when input.Length == 16 && input.Substring(4, 2) == "03" => GeocoderDatasource.NlBagLegacyPosting,
            string when input.Length == 15 && input.Substring(3, 2) == "10" => GeocoderDatasource.NlBagLegacyBuilding,
            string when input.Length == 15 && input.Substring(3, 2) == "20" => GeocoderDatasource.NlBagLegacyAddress,
            string when input.Length == 15 && input.Substring(3, 2) == "02" => GeocoderDatasource.NlBagLegacyBerth,
            string when input.Length == 15 && input.Substring(3, 2) == "03" => GeocoderDatasource.NlBagLegacyPosting,
            _ => GeocoderDatasource.Unknown,
        };

    // TODO: Add residence
    /// <summary>
    ///     Identify the geocoder datasource from the input and return repaired identifier.
    /// </summary>
    /// <remarks>
    ///     Try and fix invalid BAG identifiers. This should not be necessary however
    ///     malformed BAG identifiers are common.
    /// </remarks>
    /// <param name="input">Input identifier.</param>
    /// <param name="output">Output identifier.</param>
    /// <returns>Geocoder datasource via <see cref="GeocoderDatasource"/>.</returns>
    private static GeocoderDatasource FromIdentifier(string input, out string output)
    {
        output = input.Normalize().Replace(" ", "");

        var source = FromIdentifier(output);
        switch (source)
        {
            case GeocoderDatasource.NlBagLegacyBuilding:
                if (input.Length == 16)
                {
                    output = $"NL.IMBAG.PAND.{output}";
                }
                else if (input.Length == 15)
                {
                    output = $"NL.IMBAG.PAND.0{output}";
                }
                return GeocoderDatasource.NlBagBuilding;

            case GeocoderDatasource.NlBagLegacyAddress:
                if (input.Length == 16)
                {
                    output = $"NL.IMBAG.NUMMERAANDUIDING.{output}";
                }
                else if (input.Length == 15)
                {
                    output = $"NL.IMBAG.NUMMERAANDUIDING.0{output}";
                }
                return GeocoderDatasource.NlBagAddress;

            case GeocoderDatasource.NlBagLegacyBerth:
                if (input.Length == 16)
                {
                    output = $"NL.IMBAG.LIGPLAATS.{output}";
                }
                else if (input.Length == 15)
                {
                    output = $"NL.IMBAG.LIGPLAATS.0{output}";
                }
                return GeocoderDatasource.NlBagBerth;

            case GeocoderDatasource.NlBagLegacyPosting:
                if (input.Length == 16)
                {
                    output = $"NL.IMBAG.STANDPLAATS.{output}";
                }
                else if (input.Length == 15)
                {
                    output = $"NL.IMBAG.STANDPLAATS.0{output}";
                }
                return GeocoderDatasource.NlBagPosting;
        }

        return source;
    }

    /// <summary>
    ///     Convert geocoder identifier to address entity.
    /// </summary>
    /// <remarks
    ///    Accepts only address identifiers for either BAG or FunderMaps.
    /// <remarks>
    /// <param name="input">Input identifier.</param>
    /// <returns>If found returns the <see cref="Address"/> entity.</returns>
    public async Task<Address> GetAddressIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        // TODO: Add NlBagResidence
        GeocoderDatasource.FunderMaps => throw new EntityNotFoundException("Requested address entity could not be found."),
        GeocoderDatasource.NlBagAddress => await addressRepository.GetByExternalIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await addressRepository.GetByExternalBuildingIdAsync(id),
        _ => throw new EntityNotFoundException("Requested address entity could not be found."),
    };

    /// <summary>
    ///     Convert geocoder identifier to building entity.
    /// </summary>
    /// <remarks>
    ///    Accepts building and address identifiers for either BAG or building identifier FunderMaps.
    /// <remarks>
    /// <param name="input">Input identifier.</param>
    /// <returns>If found returns the <see cref="Building"/> entity.</returns>
    public async Task<Building> GetBuildingIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        // TODO: Add NlBagResidence
        GeocoderDatasource.FunderMaps => throw new EntityNotFoundException("Requested building entity could not be found."),
        GeocoderDatasource.NlBagAddress => await buildingRepository.GetByExternalAddressIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await buildingRepository.GetByExternalIdAsync(id),
        GeocoderDatasource.FundermapsIncidentReport => await buildingRepository.GetByIncidentIdAsync(id), // FUTURE: Maybe just request incident repository?
        _ => throw new EntityNotFoundException("Requested building entity could not be found."),
    };

    public async Task<Residence> GetResidenceIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        GeocoderDatasource.NlBagResidence => await residenceRepository.GetByIdAsync(id),
        GeocoderDatasource.NlBagAddress => await residenceRepository.GetByExternalAddressIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await residenceRepository.GetByExternalBuildingIdAsync(id),
        _ => throw new EntityNotFoundException("Requested building entity could not be found."),
    };

    /// <summary>
    ///     Convert geocoder identifier to neighborhood entity.
    /// </summary>
    /// <remarks>
    ///    Accepts neighborhood identifiers for either CBS or neighborhood identifier FunderMaps.
    /// <remarks>
    /// <param name="input">Input identifier.</param>
    /// <returns>If found returns the <see cref="Neighborhood"/> entity.</returns>
    public async Task<Neighborhood> GetNeighborhoodIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        GeocoderDatasource.FunderMaps => await neighborhoodRepository.GetByIdAsync(id), // FUTURE: This will become obsolete
        GeocoderDatasource.NlBagAddress => await neighborhoodRepository.GetByExternalAddressIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await neighborhoodRepository.GetByExternalBuildingIdAsync(id),
        GeocoderDatasource.NlCbsNeighborhood => await neighborhoodRepository.GetByExternalIdAsync(id),
        // TODO: Add Postcode
        _ => throw new EntityNotFoundException("Requested neighborhood entity could not be found."),
    };

    public async Task<District> GetDistrictIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        GeocoderDatasource.FunderMaps => await districtRepository.GetByIdAsync(id), // FUTURE: This will become obsolete
        GeocoderDatasource.NlBagAddress => await districtRepository.GetByExternalAddressIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await districtRepository.GetByExternalBuildingIdAsync(id),
        GeocoderDatasource.NlCbsNeighborhood => await districtRepository.GetByExternalNeighborhoodIdAsync(id),
        GeocoderDatasource.NlCbsDistrict => await districtRepository.GetByExternalIdAsync(id),
        // TODO: Add Postcode
        _ => throw new EntityNotFoundException("Requested district entity could not be found."),
    };

    public async Task<Municipality> GetMunicipalityIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        GeocoderDatasource.FunderMaps => await municipalityRepository.GetByIdAsync(id), // FUTURE: This will become obsolete
        GeocoderDatasource.NlBagAddress => await municipalityRepository.GetByExternalAddressIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await municipalityRepository.GetByExternalBuildingIdAsync(id),
        GeocoderDatasource.NlCbsNeighborhood => await municipalityRepository.GetByExternalNeighborhoodIdAsync(id),
        GeocoderDatasource.NlCbsDistrict => await municipalityRepository.GetByExternalDistrictIdAsync(id),
        GeocoderDatasource.NlCbsMunicipality => await municipalityRepository.GetByExternalIdAsync(id),
        // TODO: Add Postcode
        _ => throw new EntityNotFoundException("Requested municipality entity could not be found."),
    };

    public async Task<State> GetStateIdAsync(string input) => FromIdentifier(input, out string id) switch
    {
        GeocoderDatasource.FunderMaps => await stateRepository.GetByIdAsync(id), // FUTURE: This will become obsolete
        GeocoderDatasource.NlBagAddress => await stateRepository.GetByExternalAddressIdAsync(id),
        GeocoderDatasource.NlBagBuilding => await stateRepository.GetByExternalBuildingIdAsync(id),
        GeocoderDatasource.NlCbsNeighborhood => await stateRepository.GetByExternalNeighborhoodIdAsync(id),
        GeocoderDatasource.NlCbsDistrict => await stateRepository.GetByExternalDistrictIdAsync(id),
        GeocoderDatasource.NlCbsMunicipality => await stateRepository.GetByExternalMunicipalityIdAsync(id),
        GeocoderDatasource.NlCbsState => await stateRepository.GetByExternalIdAsync(id),
        // TODO: Add Postcode
        _ => throw new EntityNotFoundException("Requested state entity could not be found."),
    };
}
