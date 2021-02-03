using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.Cli.Drivers
{
    /// <summary>
    ///     gRPC channel factory.
    /// </summary>
    internal class BatchDriver : CommandDriver
    {
        private async Task StatusAsync(CancellationToken token = default) => await Task.CompletedTask;

        private async Task BuildBundleAsync(IEnumerable<Guid> bundleIdList, CancellationToken token = default) => await Task.CompletedTask;

        private async Task BuildAllAsync(CancellationToken token = default) => await Task.CompletedTask;

        #region Factory Methods

        public static Task StatusAsync(FileInfo config, IHost host)
            => ResolveSelfScope<BatchDriver>(host.Services, self => self.StatusAsync());

        public static Task BuildBundleAsync(FileInfo config, IHost host, IEnumerable<Guid> bundleId)
            => ResolveSelfScope<BatchDriver>(host.Services, self => self.BuildBundleAsync(bundleId));

        public static Task BuildAllAsync(FileInfo config, IHost host)
            => ResolveSelfScope<BatchDriver>(host.Services, self => self.BuildAllAsync());

        #endregion Factory Methods
    }
}
