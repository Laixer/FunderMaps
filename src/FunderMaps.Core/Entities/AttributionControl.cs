namespace FunderMaps.Core.Entities
{
    /// <summary>
    ///     Attribution control.
    /// </summary>
    public abstract class AttributionControl : AccessControl
    {
        /// <summary>
        ///     Attribution key.
        /// </summary>
        public int Attribution { get; set; }

        /// <summary>
        ///     Attribution object.
        /// </summary>
        public Attribution AttributionNavigation { get; set; }
    }
}
