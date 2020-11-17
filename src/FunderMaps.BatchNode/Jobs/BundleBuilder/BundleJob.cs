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

#pragma warning disable CA1812 // Internal class is never instantiated
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

        private static readonly FormatProperty[] exportFormats =
        {
            new FormatProperty
            {
                Format = GeometryFormat.MapboxVectorTiles,
                FormatName = "MVT",
                FormatShortName = "MVT",
                CommandOptions = "-dsco MINZOOM=13 -dsco MAXZOOM=16 -dsco COMPRESS=NO",
                ContentType = "application/x-protobuf",
            },
            new FormatProperty
            {
                Format = GeometryFormat.GeoPackage,
                FormatName = "GPKG",
                FormatShortName = "GPKG",
                Extension = ".gpkg",
                ContentType = "application/vnd.sqlite3",
            },
            new FormatProperty
            {
                Format = GeometryFormat.ESRIShapefile,
                FormatName = "ESRI Shapefile",
                FormatShortName = "SHP",
                Extension = ".shp",
                ContentType = "x-gis/x-shapefile",
            },
            new FormatProperty
            {
                Format = GeometryFormat.GeoJSON,
                FormatName = "GeoJSON",
                FormatShortName = "JSON",
                Extension = ".json",
                ContentType = "application/json",
            },
        };

        private string connectionString;

        /// <summary>
        ///     Geometry format properties.
        /// </summary>
        record FormatProperty
        {
            /// <summary>
            ///     Geometry format.
            /// </summary>
            public GeometryFormat Format { get; init; }

            /// <summary>
            ///     Format name as used in system calls.
            /// </summary>
            public string FormatName { get; init; }

            /// <summary>
            ///     Format short name used in directories.
            /// </summary>
            public string FormatShortName { get; init; }

            /// <summary>
            ///     Format file extension.
            /// </summary>
            public string Extension { get; init; }

            /// <summary>
            ///     Format dataset options.
            /// </summary>
            public string CommandOptions { get; init; }

            /// <summary>
            ///     Format file content type.
            /// </summary>
            public string ContentType { get; init; }
        }

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

        private async Task<DataSource> BuildBundleWithLayerAsync(Bundle bundle, IEnumerable<Layer> layers, DataSource input, GeometryFormat format)
        {
            FormatProperty formatProperty = exportFormats.First(f => f.Format == format);
            string blobStoragePath = $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/{formatProperty.FormatShortName}";

            FileDataSource fileDump = new()
            {
                Format = format,
                PathPrefix = CreateDirectory(formatProperty.FormatShortName),
                Extension = formatProperty.Extension,
                Name = bundle.Id.ToString(),
            };

            foreach (var layer in layers)
            {
                var command = new VectorDatasetBuilder(
                    new()
                    {
                        AdditionalOptions = formatProperty.CommandOptions,
                        Append = true,
                    })
                    .InputDataset(input)
                    .InputLayers(new BundleLayerSource(bundle, layer))
                    .OutputDataset(fileDump)
                    .Build(formatProperty.FormatName);

                await RunCommandAsync(command);
            }

            _logger.LogTrace($"Start uploading exported bundle");

            await _blobStorageService.StoreDirectoryAsync(
                directoryName: blobStoragePath,
                directoryPath: fileDump.PathPrefix, new Core.Storage.StorageObject
                {
                    ContentType = formatProperty.ContentType,
                    CacheControl = "public, max-age=3600",
                    IsPublic = true,
                });

            _logger.LogInformation($"Export of format {formatProperty.FormatName} done");

            return fileDump;
        }

        private async Task<DataSource> BuildBundleAsync(Bundle bundle, DataSource input, GeometryFormat format)
        {
            FormatProperty formatProperty = exportFormats.First(f => f.Format == format);
            string blobStoragePath = $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/{formatProperty.FormatShortName}";

            FileDataSource fileDump = new()
            {
                Format = format,
                PathPrefix = CreateDirectory(formatProperty.FormatShortName),
                Extension = formatProperty.Extension,
                Name = bundle.Id.ToString(),
            };

            var command = new VectorDatasetBuilder(
                new()
                {
                    AdditionalOptions = formatProperty.CommandOptions,
                    Overwrite = true,
                })
                .InputDataset(input)
                .OutputDataset(fileDump)
                .Build(formatProperty.FormatName);

            await RunCommandAsync(command);

            _logger.LogTrace($"Start uploading exported bundle");

            await _blobStorageService.StoreDirectoryAsync(
                directoryName: blobStoragePath,
                directoryPath: fileDump.PathPrefix, new Core.Storage.StorageObject
                {
                    ContentType = formatProperty.ContentType,
                    CacheControl = "public, max-age=3600",
                    IsPublic = true,
                });

            _logger.LogInformation($"Export of format {formatProperty.FormatName} done");

            return fileDump;
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (JsonSerializer.Deserialize<BundleBuildingContext>(context.Value as string) is not BundleBuildingContext bundleBuildingContext)
            {
                throw new ProtocolException("Invalid bundle building context");
            }

            if (bundleBuildingContext.Formats is null || !bundleBuildingContext.Formats.Any())
            {
                _logger.LogWarning("No formats listed for export");
                return;
            }

            var formats = bundleBuildingContext.Formats.Distinct().ToList();
            formats.RemoveAll(f => f == GeometryFormat.GeoPackage);

            Bundle bundle = await _bundleRepository.GetByIdAsync(bundleBuildingContext.BundleId);
            IList<Layer> layers = await _layerRepository.ListAllFromBundleIdAsync(bundleBuildingContext.BundleId).ToListAsync();

            DataSource localCacheDataSource = await BuildBundleWithLayerAsync(bundle, layers,
                input: PostreSQLDataSource.FromConnectionString(connectionString),
                format: GeometryFormat.GeoPackage);

            foreach (var format in formats)
            {
                await BuildBundleAsync(bundle, localCacheDataSource, format);
            }
        }

        /// <summary>
        ///     Method to check if a task can be handeld by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToLowerInvariant() == TaskName && value is string;
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
