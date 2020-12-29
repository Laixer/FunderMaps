using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.MapBundle;
using FunderMaps.Core.Threading;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Hosting;

namespace FunderMaps.Cli.Drivers
{
    /// <summary>
    ///     gRPC channel factory.
    /// </summary>
    internal class BatchDriver : CommandDriver
    {
        private readonly IBatchService _batchService;
        private readonly IBundleService _bundleService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BatchDriver(IBatchService batchService, IBundleService bundleService)
            => (_batchService, _bundleService) = (batchService, bundleService);

        private async Task StatusAsync(CancellationToken token = default)
        {
            DispatchManagerStatus status = await _batchService.StatusAsync(token);
            Console.WriteLine($"Jobs failed: {status.JobsFailed}");
            Console.WriteLine($"Jobs succeeded: {status.JobsSucceeded}");
        }

        private async Task BuildBundleAsync(IEnumerable<Guid> bundleIdList, CancellationToken token = default)
        {
            foreach (var bundleId in bundleIdList)
            {
                await _bundleService.BuildAsync(new BundleBuildingContext
                {
                    BundleId = bundleId,
                    Formats = new List<GeometryFormat> { GeometryFormat.GeoPackage },
                });
            }
        }

        private async Task BuildAllAsync(CancellationToken token = default)
        {
            await _bundleService.BuildAllAsync(new BundleBuildingContext
            {
                Formats = new List<GeometryFormat> { GeometryFormat.GeoPackage },
            });
        }

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
