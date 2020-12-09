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
        private const string TaskName = "BUNDLE_BUILDING";

        protected readonly IBundleRepository _bundleRepository;
        protected readonly ILayerRepository _layerRepository;
        protected readonly IBlobStorageService _blobStorageService;

        private static readonly FormatProperty[] exportFormats =
        {
            new FormatProperty
            {
                Format = GeometryFormat.MapboxVectorTiles,
                FormatName = "MVT",
                FormatShortName = "MVT",
                CommandOptions = "-dsco MINZOOM=14 -dsco MAXZOOM=16 -dsco COMPRESS=NO -dsco MAX_SIZE=25000000",
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
        public BundleJob(
            IConfiguration configuration,
            IBundleRepository bundleRepository,
            ILayerRepository layerRepository,
            IBlobStorageService blobStorageService,
            ILogger<BundleJob> logger)
            : base(logger)
        {
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
            _layerRepository = layerRepository ?? throw new ArgumentNullException(nameof(layerRepository));
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));

            connectionString = configuration.GetConnectionString("FunderMapsConnection");
        }

        /// <summary>
        ///     Build the geometry dataset from bundle.
        /// </summary>
        /// <param name="bundle">Bundle to build.</param>
        /// <param name="layers">Layers to include in the output dataset.</param>
        /// <param name="input">Dataset input.</param>
        /// <param name="format">Output format.</param>
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

            int returnCode = 0;
            foreach (var layer in layers)
            {
                // NOTE: Only check the return code in the next iteration. We cannot append to an
                //       invalid dataset, however we can continue if only the last layer failed to
                //       complete.
                if (returnCode != 0)
                {
                    throw new ProcessException("Last layer command failed, refuse to continue");
                }

                var command = new VectorDatasetBuilder(
                    new()
                    {
                        AdditionalOptions = formatProperty.CommandOptions,
                        Append = true,
                    })
                    .InputDataset(input)
                    .InputLayers(new BundleLayerSource(bundle, layer, Context.Workspace))
                    .OutputDataset(fileDump)
                    .Build(formatProperty.FormatName);

                returnCode = await RunCommandAsync(command);
            }

            Logger.LogTrace($"Start uploading exported bundle");

            await _blobStorageService.StoreDirectoryAsync(
                directoryName: blobStoragePath,
                directoryPath: fileDump.PathPrefix, new Core.Storage.StorageObject
                {
                    ContentType = formatProperty.ContentType,
                    CacheControl = "public, max-age=3600",
                    IsPublic = true,
                });

            Logger.LogInformation($"Export of format {formatProperty.FormatName} done");

            return fileDump;
        }

        /// <summary>
        ///     Build the geometry dataset from bundle.
        /// </summary>
        /// <param name="bundle">Bundle to build.</param>
        /// <param name="input">Dataset input.</param>
        /// <param name="format">Output format.</param>
        private async Task<DataSource> BuildBundleAsync(Bundle bundle, DataSource input, GeometryFormat format)
        {
            FormatProperty formatProperty = exportFormats.First(f => f.Format == format);
            var blobStoragePath = $"dist/ORG{bundle.OrganizationId}/BND{bundle.Id}/{formatProperty.FormatShortName}";

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

            Logger.LogTrace($"Start uploading exported bundle");

            await _blobStorageService.StoreDirectoryAsync(
                directoryName: blobStoragePath,
                directoryPath: fileDump.PathPrefix, new Core.Storage.StorageObject
                {
                    ContentType = formatProperty.ContentType,
                    CacheControl = "public, max-age=3600",
                    IsPublic = true,
                });

            Logger.LogInformation($"Export of format {formatProperty.FormatName} done");

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

            var bundleBuildingContext = JsonSerializer.Deserialize<BundleBuildingContext>(context.Value as string, new()
            {
                PropertyNameCaseInsensitive = true,
            });

            // NOTE: The serializer will always return an object no matter the input value. Therefore we'll check
            //       if the property BundleId was initialized to a non-empty value.
            if (bundleBuildingContext.BundleId == Guid.Empty)
            {
                throw new ProtocolException("Invalid bundle building context");
            }

            if (bundleBuildingContext.Formats is null || !bundleBuildingContext.Formats.Any())
            {
                Logger.LogWarning("No formats listed for export");
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
            => name is not null && name.ToUpperInvariant() == TaskName && value is string;
    }
}
#pragma warning restore CA1812 // Internal class is never instantiated
