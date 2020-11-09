using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FunderMaps.BatchNode.Command;
using FunderMaps.BatchNode.GeoInterface;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FunderMaps.Core.Exceptions;

namespace FunderMaps.BatchNode.Jobs.BundleBuilder
{
    /// <summary>
    ///     Bundle job entry.
    /// </summary>
    internal class BundleJob : CommandTask
    {
        private const string TaskName = "bundle_building";

        protected readonly IBundleRepository _bundleRepository;
        protected readonly ILayerRepository _layerRepository;
        protected readonly IBlobStorageService _blobStorageService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IServiceScope _scope;

        private string connectionString;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleJob(IServiceScopeFactory serviceScopeFactory,
            ILogger<BundleJob> logger)
            : base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _scope = _serviceScopeFactory.CreateScope();

            // FUTURE: From injection scope
            var ctx = _scope.ServiceProvider.GetRequiredService<Core.AppContext>();
            ctx.Cache = _scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();

            _bundleRepository = _scope.ServiceProvider.GetService<IBundleRepository>();
            _layerRepository = _scope.ServiceProvider.GetService<ILayerRepository>();
            _blobStorageService = _scope.ServiceProvider.GetService<IBlobStorageService>();

            var configuration = _scope.ServiceProvider.GetRequiredService<IConfiguration>();
            connectionString = configuration.GetConnectionString("FunderMapsConnection");
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var bundleBuildingContext = JsonSerializer.Deserialize<BundleBuildingContext>(context.Value as string);
            if (bundleBuildingContext == null)
            {
                throw new ProtocolException();
            }

            if (bundleBuildingContext.Formats == null || !bundleBuildingContext.Formats.Any())
            {
                _logger.LogWarning("No formats listed for export");
                return;
            }

            var formats = bundleBuildingContext.Formats.Distinct().ToList();
            formats.RemoveAll(f => f == GeometryExportFormat.GeoPackage);

            Bundle bundle = await _bundleRepository.GetByIdAsync(bundleBuildingContext.BundleId);
            IList<Layer> layers = await _layerRepository.ListAllFromBundleIdAsync(bundleBuildingContext.BundleId).ToListAsync();

            var localCacheDataSource = new FileDataSource
            {
                Format = GeometryExportFormat.GeoPackage,
                PathPrefix = CreateDirectory("GPKG"),
                Name = bundle.Id.ToString(),
            };

            // FUTURE: Reduce to single op.
            // NOTE: For now we're processing all configured formats

            {
                var command = new VectorDatasetBuilder()
                    .InputDataset(PostreSQLDataSource.FromConnectionString(connectionString))
                    .InputLayers(new BundleLayerSource(bundle, layers.First()))
                    .OutputDataset(localCacheDataSource)
                    .Build();

                await RunCommandAsync(command);

                await _blobStorageService.StoreDirectoryAsync(
                    directoryName: $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/GPKG",
                    directoryPath: localCacheDataSource.PathPrefix, new Core.Storage.StorageObject
                    {
                        ContentType = "application/vnd.sqlite3",
                        CacheControl = "public, max-age=3600",
                        IsPublic = true,
                    });
            }

            // FUTURE: Content compression is disabled. 

            {
                var fileDump = new FileDataSource()
                {
                    Format = GeometryExportFormat.MapboxVectorTiles,
                    PathPrefix = CreateDirectory("MVT"),
                    Name = bundle.Id.ToString(),
                };

                var command = new VectorDatasetBuilder(
                    new VectorDatasetBuilderOptions
                    {
                        AdditionalOptions = "-dsco MINZOOM=7 -dsco MAXZOOM=15 -dsco COMPRESS=NO",
                    })
                    .InputDataset(localCacheDataSource)
                    .OutputDataset(fileDump)
                    .Build();

                await RunCommandAsync(command);

                await _blobStorageService.StoreDirectoryAsync(
                    directoryName: $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/MVT",
                    directoryPath: fileDump.ToString(), new Core.Storage.StorageObject
                    {
                        ContentType = "application/x-protobuf",
                        CacheControl = "public, max-age=3600",
                        IsPublic = true,
                    });
            }

            {
                var fileDump = new FileDataSource
                {
                    Format = GeometryExportFormat.ESRIShapefile,
                    PathPrefix = CreateDirectory("SHP"),
                    Name = bundle.Id.ToString(),
                };

                var command = new VectorDatasetBuilder()
                    .InputDataset(localCacheDataSource)
                    .OutputDataset(fileDump)
                    .Build();

                await RunCommandAsync(command);

                await _blobStorageService.StoreDirectoryAsync(
                    directoryName: $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/SHP",
                    directoryPath: fileDump.PathPrefix, new Core.Storage.StorageObject
                    {
                        ContentType = "x-gis/x-shapefile",
                        CacheControl = "public, max-age=3600",
                        IsPublic = true,
                    });
            }

            {
                var fileDump = new FileDataSource
                {
                    Format = GeometryExportFormat.GeoJSON,
                    PathPrefix = CreateDirectory("JSON"),
                    Name = bundle.Id.ToString(),
                };

                var command = new VectorDatasetBuilder()
                    .InputDataset(localCacheDataSource)
                    .OutputDataset(fileDump)
                    .Build();

                await RunCommandAsync(command);

                await _blobStorageService.StoreDirectoryAsync(
                    directoryName: $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/JSON",
                    directoryPath: fileDump.PathPrefix, new Core.Storage.StorageObject
                    {
                        ContentType = "application/json",
                        CacheControl = "public, max-age=3600",
                        IsPublic = true,
                    });
            }
        }

        /// <summary>
        ///     Method to check if a task can be handeld by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="valdirectoryNameue">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name.ToLowerInvariant() == TaskName && value is string;
    }
}
