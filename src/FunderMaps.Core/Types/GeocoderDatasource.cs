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
        ///     Basis Registratie Gebouwen (BAG) legacy building identifier.
        /// </summary>
        NlBagLegacyBuilding = 8,

        /// <summary>
        ///     Basis Registratie Gebouwen (BAG) legacy address identifier.
        /// </summary>
        NlBagLegacyAddress = 9,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) neighborhoor identifier.
        /// </summary>
        NlCbsNeighborhood = 15,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) district identifier.
        /// </summary>
        NlCbsDistrict = 16,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) municipality identifier.
        /// </summary>
        NlCbsMunicipality = 17,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS) state identifier.
        /// </summary>
        NlCbsState = 18,

        /// <summary>
        ///     Unknown identifier.
        /// </summary>
        Unknown = 50,
    }
}
