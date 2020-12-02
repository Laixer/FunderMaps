using System;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;

namespace FunderMaps.Core.Components
{
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
                string when input.StartsWith("NL.IMBAG.NUMMERAANDUIDING", StringComparison.InvariantCultureIgnoreCase) => GeocoderDatasource.NlBagBuilding,
                string when input.StartsWith("BU", StringComparison.InvariantCultureIgnoreCase) && input.Length == 10 => GeocoderDatasource.NlCbsNeighborhood,
                string when input.StartsWith("WK", StringComparison.InvariantCultureIgnoreCase) && input.Length == 8 => GeocoderDatasource.NlCbsDistrict,
                string when input.StartsWith("GM", StringComparison.InvariantCultureIgnoreCase) && input.Length == 6 => GeocoderDatasource.NlCbsMunicipality,
                string when input.StartsWith("PV", StringComparison.InvariantCultureIgnoreCase) && input.Length == 4 => GeocoderDatasource.NlCbsState,
                _ => GeocoderDatasource.Unknown,
            };
    }
}
