using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Components;

/// <summary>
///     Identify the geocoder datasource from the input.
/// </summary>
public class GeocoderParser : IGeocoderParser
{
    /// <summary>
    ///     Identify the geocoder datasource from the input.
    /// </summary>
    /// <param name="input">Input identifier.</param>
    /// <returns>Geocoder datasource via <see cref="GeocoderDatasource"/>.</returns>
    public virtual GeocoderDatasource FromIdentifier(string input)
        => input switch
        {
            string when input.StartsWith("gfm-", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.FunderMaps,
            string when input.StartsWith("NL.IMBAG.PAND", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagBuilding,
            string when input.StartsWith("NL.IMBAG.LIGPLAATS", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagBerth,
            string when input.StartsWith("NL.IMBAG.STANDPLAATS", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagPosting,
            string when input.StartsWith("NL.IMBAG.NUMMERAANDUIDING", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagAddress,
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
    public virtual GeocoderDatasource FromIdentifier(string input, out string output)
    {
        output = input;

        GeocoderDatasource source = FromIdentifier(input);
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
}
