using System;
using System.Collections.Generic;
using System.Threading;

namespace FunderMaps.Core
{
    /// <summary>
    ///     Application context.
    /// </summary>
    public class AppContext
    {
        /// <summary>
        ///     Notifies when this call is aborted and thus request operations should be cancelled.
        /// </summary>
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        /// <summary>
        ///     Gets or sets the service provider that provides access to the service container.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///     Gets or sets a key/value collection that can be used to share data within this scope.
        /// </summary>
        public Dictionary<object, object> Items { get; set; }

        // TODO:
        // - User
        // - Org
        // - Culture
    }
}
