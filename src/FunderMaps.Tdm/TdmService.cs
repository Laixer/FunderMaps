using Microsoft.Extensions.Options;
using System;
using TdmClient;

namespace FunderMaps.Tdm;
{
    /// <summary>
    /// Wrapper around TdmClient.
    /// </summary>
    /// <remarks>
    /// This service lets other services depend on this component via DI.
    /// </remarks>
    public class TdmService
    {
        /// <summary>
        /// Tdm service configuration.
        /// </summary>
        protected TdmConfig Config { get; private set; }

        /// <summary>
        /// Access to TdmClient sync services.
        /// </summary>
        public TdmSyncService SyncService { get; }

        /// <summary>
        /// Access to TdmClient media services.
        /// </summary>
        public TdmMediaService MediaService { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="options">Service configuration.</param>
        public TdmService(IOptions<TdmServiceOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Config = options.Value.Config ?? throw new ArgumentNullException(nameof(options));

            SyncService = new TdmSyncService(Config);
            MediaService = new TdmMediaService(Config);
        }
    }
}
