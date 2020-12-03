namespace FunderMaps.Core.Types
{
    /// <summary>
    ///     Represents an external data source.
    /// </summary>
    public enum ExternalDataSource
    {
        /// <summary>
        ///     Basis Registratie Gebouwen (BAG).
        /// </summary>
        NlBag = 0,

        /// <summary>
        ///     Open Street Maps (OSM).
        /// </summary>
        NlOsm = 1,

        /// <summary>
        ///     Centraal Bureau Statistiek (CBS).
        /// </summary>
        NlCbs = 2,
    }
}
