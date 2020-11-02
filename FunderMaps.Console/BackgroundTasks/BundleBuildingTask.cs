using FunderMaps.Console.Types;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Core.Types.BackgroundTasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunderMaps.Console.BundleServices
{
    ///
    /// TODO:
    ///     - We need to build mvt from gpkg because we can't add specific layers to an
    ///       mvt file command. We also can't enforce the geofence.
    ///     - Build gpkg first
    ///     - If mvt, then also add gpkg
    /// 
    /// <summary>
    ///     Service for exporting bundles.
    /// </summary>
    internal class BundleBuildingTask : BackgroundTaskBase
    {
        protected readonly IBundleRepository _bundleRepository;
        protected readonly ILayerRepository _layerRepository;
        protected readonly BundleStorageService _bundleStorageService;
        protected readonly BundleBuildingOptions _options;
        protected readonly DbConnectionStringBuilder csBuilder = new DbConnectionStringBuilder();

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleBuildingTask(IBundleRepository bundleRepository,
            ILayerRepository layerRepository,
            BundleStorageService bundleStorageService,
            IOptions<BundleBuildingOptions> options,
            IConfiguration configuration)
        {
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
            _layerRepository = layerRepository ?? throw new ArgumentNullException(nameof(layerRepository));
            _bundleStorageService = bundleStorageService ?? throw new ArgumentNullException(nameof(bundleStorageService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            csBuilder.ConnectionString = configuration?.GetConnectionString(_options.ConnectionStringName) ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        ///     Builds a single bundle into it's most up-to-date desired formats.
        /// </summary>
        /// <remarks>
        ///     This will always recompute all formats.
        /// </remarks>
        /// <param name="bundleId">The bundle id.</param>
        /// <param name="formats">The formats to export the bundle to.</param>
        public override async Task ProcessAsync(BackgroundTaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var bundleBuildingContext = (BundleBuildingContext)context.Value;
            bundleBuildingContext.BundleId.ThrowIfNullOrEmpty();
            if (bundleBuildingContext.Formats == null || !bundleBuildingContext.Formats.Any())
            {
                throw new ArgumentNullException(nameof(bundleBuildingContext.Formats));
            }
            if (bundleBuildingContext.Formats.Distinct().Count() != bundleBuildingContext.Formats.Count())
            {
                // TODO Warning?
                throw new InvalidOperationException("Can't have duplicates in format list");
            }

            // To calculate the mvt we always need a gpkg.
            // TODO This is ugly.
            var formats = bundleBuildingContext.Formats.ToList();
            if (bundleBuildingContext.Formats.Contains(GeometryExportFormat.Mvt) && !bundleBuildingContext.Formats.Contains(GeometryExportFormat.Gpkg))
            {
                formats.Add(GeometryExportFormat.Gpkg);
            }

            var bundle = await _bundleRepository.GetByIdAsync(bundleBuildingContext.BundleId).AsTask();
            var layers = await _layerRepository.ListAllFromBundleIdAsync(bundleBuildingContext.BundleId).ToListAsync();

            ExportFormats(bundle, layers, formats);
        }

        /// <summary>
        ///     Checks if we can handle the value object.
        /// </summary>
        /// <param name="value">The value object.</param>
        /// <returns>Boolean answer.</returns>
        public override bool CanHandle(object value)
            => value is BundleBuildingContext;

        /// <summary>
        ///     Exports a bundle to a list of <see cref="GeometryExportFormat"/>s.
        /// </summary>
        /// <remarks>
        ///     This will always start with <see cref="GeometryExportFormat.Gpkg"/>
        ///     if the <paramref name="formats"/> contain one.
        /// </remarks>
        /// <param name="bundle">The bundle to export.</param>
        /// <param name="layers">The bundles layers.</param>
        /// <param name="formats">All desired export formats.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        private void ExportFormats(Bundle bundle, IEnumerable<Layer> layers, IList<GeometryExportFormat> formats)
        {
            // Always start with the .gpkg if we have it
            if (formats.Contains(GeometryExportFormat.Gpkg))
            {
                Export(bundle, layers, GeometryExportFormat.Gpkg);
                formats.Remove(GeometryExportFormat.Gpkg);
            }

            foreach (var format in formats)
            {
                Export(bundle, layers, format);
            }
        }

        /// <summary>
        ///     Exports a bundle and all its layers to a specified format.
        /// </summary>
        /// <param name="bundle">The bundle to export.</param>
        /// <param name="layers">The bundles layers.</param>
        /// <param name="format">The desired export format.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        private void Export(Bundle bundle, IEnumerable<Layer> layers, GeometryExportFormat format)
        {
            switch (format)
            {
                case GeometryExportFormat.Mvt:
                    ExportMvtFromExistingGpkg(bundle);
                    break;
                case GeometryExportFormat.Gpkg:
                    ExportGpkg(bundle, layers.ToList());
                    break;
                case GeometryExportFormat.Shp:
                    throw new NotImplementedException(nameof(format));
                case GeometryExportFormat.GeoJSON:
                    throw new NotImplementedException(nameof(format));
                default:
                    throw new InvalidOperationException(nameof(format));
            }
        }

        /// <summary>
        ///     Exports a bundle and all its layers to MVT format.
        /// </summary>
        /// <param name="bundle">The bundle to export.</param>
        /// <param name="layers">Collection of the bundles layers.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        private void ExportGpkg(Bundle bundle, IList<Layer> layers)
        {
            // Process the first layer, then append.
            ProcessLayer(layers[0], false);

            // Append the rest of the layers
            for (int i = 1; i < layers.Count; i++)
            {
                ProcessLayer(layers[i], true);
            }

            // When all layers are appended we can upload to storage.
            _bundleStorageService.UploadExport(bundle.OrganizationId, bundle.Id, bundle.VersionId, GeometryExportFormat.Gpkg);

            // Nested function that exports an sql statement into a GPKG.
            void ProcessLayer(Layer layer, bool append)
            {
                var sql = GetSqlFromLayer(bundle, layer);

                // TODO Name thing already exists?
                var fileName = $"{BundleNameHelper.GetName(bundle, GeometryExportFormat.Gpkg)}";

                // TODO Centralize ogr2ogr
                var commandText = $"ogr2ogr -f GPKG {fileName} {BuildPGString()} {(append ? "-append " : "")}-sql \"{sql}\" -nln \"{layer.SchemaName}.{layer.TableName}\"";

                CommandExecuter.ExecuteCommand(commandText);
            }
        }

        /// <summary>
        ///     Exports a bundle and all its layers to GPKG format.
        /// </summary>
        /// <remarks>
        ///     This expects a .gpkg file to exist in its working directory.
        ///     The MVT files will be stored to a directory, not to a file.
        /// </remarks>
        /// <param name="bundle">The bundle to export.</param>
        private void ExportMvtFromExistingGpkg(Bundle bundle)
        {
            // TODO Validate gpkg has the same layers as param layers?

            var sourceFile = $"{BundleNameHelper.GetName(bundle, GeometryExportFormat.Gpkg)}";
            var targetDirectory = $"{StorageConstants.ExportMvtDirectoryName}";
            var mvtOptions = $"-dsco MINZOOM={_options.MvtMinZoom} -dsco MAXZOOM={_options.MvtMaxZoom}";

            var commandText = $"ogr2ogr -f MVT {targetDirectory} {sourceFile} {mvtOptions}";

            CommandExecuter.ExecuteCommand(commandText);

            _bundleStorageService.UploadExportDirectory(bundle.OrganizationId, bundle.Id, bundle.VersionId, GeometryExportFormat.Mvt);
        }

        /// <summary>
        ///     Builds an SQL statement based on the specified layer.
        /// </summary>
        /// <remarks>
        ///     This also takes the <see cref="BundleServices.OrganizationId"/> fence
        ///     into account when mapping all the geometries.
        /// </remarks>
        /// <param name="bundle">The bundle to export for.</param>
        /// <param name="layer">The corresponding layer.</param>
        /// <returns>SQL statement based on the layer configuration.</returns>
        private string GetSqlFromLayer(Bundle bundle, Layer layer)
        {
            var configuration = bundle.LayerConfiguration.Layers.Where(x => x.LayerId == layer.Id).FirstOrDefault();
            if (configuration == null)
            {
                throw new LayerConfigurationException(nameof(bundle.LayerConfiguration.Layers));
            }
            if (!configuration.ColumnNames.Any())
            {
                throw new LayerConfigurationException(nameof(configuration.ColumnNames));
            }
            if (!configuration.ColumnNames.Contains("geom"))
            {
                throw new LayerConfigurationException($"Geometry column with name 'geom' must be selected for layer {layer.Id} {layer.SchemaName}.{layer.TableName}");
            }

            var columnNames = configuration.ColumnNames.ToList();

            var sb = new StringBuilder();

            sb.Append("SELECT");

            for (int i = 0; i < columnNames.Count(); i++)
            {
                sb.Append($"\n\ts.{columnNames[i]}");
                if (i != columnNames.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append($"\nFROM {layer.SchemaName}.{layer.TableName} AS s");
            sb.Append($"\nJOIN application.organization AS o ON o.id = '{bundle.OrganizationId}'");
            sb.Append("\nWHERE st_intersects(o.fence, s.geom)");

            return sb.ToString();
        }

        // TODO Might be too hard coded
        /// <summary>
        ///     Builds a PG string for the ogr2ogr tool based on our options.
        /// </summary>
        /// <returns>The created PG string.</returns>
        private string BuildPGString()
            => $"PG:\"host={(string)csBuilder["Host"]} dbname={(string)csBuilder["Database"]} user={(string)csBuilder["User Id"]} password={(string)csBuilder["Password"]}\"";
    }
}
