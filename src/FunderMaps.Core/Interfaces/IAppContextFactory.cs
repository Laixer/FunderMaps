namespace FunderMaps.Core.Interfaces
{
    /// <summary>
    ///     AppContext factory.
    /// </summary>
    public interface IAppContextFactory
    {
        /// <summary>
        ///     Create the <see cref="AppContext"/>.
        /// </summary>
        AppContext Create();
    }
}
