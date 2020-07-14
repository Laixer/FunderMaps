namespace FunderMaps.WebApi.ViewModels
{
    /// <summary>
    /// Output model for entity statistics
    /// </summary>
    public sealed class EntityStatsOutputModel
    {
        /// <summary>
        /// Number of entities accessable to the current principal.
        /// </summary>
        public long Count { get; set; }
    }
}
