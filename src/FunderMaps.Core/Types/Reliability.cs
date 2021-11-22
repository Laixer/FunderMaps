namespace FunderMaps.Core.Types
{
    /// <summary>
    ///     Indicates the reliability of a product.
    /// </summary>
    public enum Reliability
    {
        /// <summary>
        ///     When our model was used.
        /// </summary>
        Indicative = 0,

        /// <summary>
        ///     When a report is present.
        /// </summary>
        Established = 1,

        /// <summary>
        ///     When building from the same cluster was used.
        /// </summary>
        Cluster = 2,
    }
}
