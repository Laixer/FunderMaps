namespace FunderMaps.Core.Types;

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
    ///     Basisregistratie Adressen en Gebouwen (BAG) building identifier.
    /// </summary>
    NlBagBuilding = 1,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) address identifier.
    /// </summary>
    NlBagAddress = 2,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) posting identifier.
    /// </summary>
    NlBagPosting = 3,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) berth identifier.
    /// </summary>
    NlBagBerth = 4,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) legacy building identifier.
    /// </summary>
    NlBagLegacyBuilding = 8,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) legacy address identifier.
    /// </summary>
    NlBagLegacyAddress = 9,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) legacy posting identifier.
    /// </summary>
    NlBagLegacyPosting = 10,

    /// <summary>
    ///     Basisregistratie Adressen en Gebouwen (BAG) legacy berth identifier.
    /// </summary>
    NlBagLegacyBerth = 11,

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
