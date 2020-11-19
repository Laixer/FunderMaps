using FunderMaps.Core.Interfaces;

namespace FunderMaps.Core.Components
{
    /// <summary>
    ///     Default AppContext factory.
    /// </summary>
    public class AppContextFactory : IAppContextFactory
    {
        /// <summary>
        ///     Create the <see cref="AppContext"/>.
        /// </summary>
        public virtual AppContext Create() => new();
    }
}
