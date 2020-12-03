namespace FunderMaps.Core.Types
{
    /// <summary>
    ///     Geocoder datasource identifier.
    /// </summary>
    public enum GeocoderDatasource
    {
        /// <summary>
        ///     FunderMaps geocoder identifier (GFM).
        /// </summary>
        FunderMaps = 0,

        /// <summary>
        ///     Basis Registratie Gebouwen (BAG) building identifier.
        /// </summary>
        NlBagBuilding = 1,

        /// <summary>
        ///     Basis Registratie Gebouwen (BAG) address identifier.
        /// </summary>
        NlBagAddress = 2,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) neighborhoor identifier.
        /// </summary>
        NlCbsNeighborhood = 5,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) district identifier.
        /// </summary>
        NlCbsDistrict = 6,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) municipality identifier.
        /// </summary>
        NlCbsMunicipality = 7,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) state identifier.
        /// </summary>
        NlCbsState = 8,

        /// <summary>
        ///     Unknown identifier.
        /// </summary>
        Unknown = 10,
    }
}
