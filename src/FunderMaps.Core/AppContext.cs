using System;
using System.Collections.Generic;
using System.Threading;
using FunderMaps.Core.Identity;

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

        /// <summary>
        ///     User identity.
        /// </summary>
        public IUser User { get; set; }

        /// <summary>
        ///     Tenant identity.
        /// </summary>
        public ITenant Tenant { get; set; }

        // FUTURE: Maybe move up
        /// <summary>
        ///     User identifier.
        /// </summary>
        public Guid UserId => User.Id;

        // FUTURE: Maybe move up
        /// <summary>
        ///     Tenant identifier.
        /// </summary>
        public Guid TenantId => Tenant.Id;

        // FUTURE: Maybe move up
        /// <summary>
        ///     Indicates that identity has been set or not.
        /// </summary>
        /// <remarks>If <see cref="User"/> exists, then <see cref="Tenant"/> exists.</remarks>
        public bool HasIdentity => User != null;

        // TODO:
        // - Culture
    }
}