namespace FunderMaps.Core.Abstractions
{
    /// <summary>
    ///     Application service base.
    /// </summary>
    public abstract class AppServiceBase
    {
        /// <summary>
        ///     Application context.
        /// </summary>
        public AppContext AppContext { get; set; }
    }
}
