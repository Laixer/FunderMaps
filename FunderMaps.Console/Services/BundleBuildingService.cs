using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunderMaps.Console.Services
{
    ///
    /// TODO 
    ///     - We need to build mvt from gpkg because we can't add specific layers to an
    ///       mvt file command. We also can't enforce the geofence.
    ///     - Build gpkg first
    ///     - If mvt, then also add gpkg
    /// 
    /// <summary>
    ///     Service for exporting bundles.
    /// </summary>
    internal sealed class BundleBuildingService
    {
        private readonly IBundleRepository _bundleRepository;
        private readonly ILayerRepository _layerRepository;
        private readonly BundleStorageService _bundleStorageService;
        private readonly ConsoleOptions _options;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public BundleBuildingService(IBundleRepository bundleRepository,
            ILayerRepository layerRepository,
            BundleStorageService bundleStorageService,
            IOptions<ConsoleOptions> options)
        {
            _bundleRepository = bundleRepository ?? throw new ArgumentNullException(nameof(bundleRepository));
            _layerRepository = layerRepository ?? throw new ArgumentNullException(nameof(layerRepository));
            _bundleStorageService = bundleStorageService ?? throw new ArgumentNullException(nameof(bundleStorageService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        ///     Builds all existing bundles.
        /// </summary>
        /// <param name="formats">The desired export formats.</param>
        /// <param name="force">Force export override for all.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        public async Task BuildAllBundlesAsync(IList<GeometryExportFormat> formats, bool force = false)
        {
            if (formats == null || !formats.Any())
            {
                throw new ArgumentNullException(nameof(formats));
            }

            await foreach (var bundle in _bundleRepository.ListAllAsync(Navigation.All))
            {
                if (force)
                {
                    await ForceBuildBundleAsync(bundle.Id, formats);
                }
                else
                {
                    await BuildBundleAsync(bundle.Id, formats);
                }
            }
        }

        // TODO If we have GPKG exported but not selected on the SAME version id,
        // our job will fail because we expect the GPKG to be on the local drive.
        // This is a bug! Either always recompute or download the GPKG.

        // TODO Maybe this is too complex with the missing formats.
        /// <summary>
        ///     Builds a single bundle into it's most up-to-date desired formats.
        /// </summary>
        /// <remarks>
        ///     If the currently exported version id is equal to the bundles stored
        ///     version id in the data store, this function simply returns.
        /// </remarks>
        /// <param name="bundleId">The bundle id.</param>
        /// <param name="formats">The formats to export the bundle to.</param>
        /// <returns></returns>
        public async Task BuildBundleAsync(Guid bundleId, IList<GeometryExportFormat> formats)
        {
            if (bundleId == null || bundleId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bundleId));
            }
            if (formats == null || !formats.Any())
            {
                throw new ArgumentNullException(nameof(formats));
            }
            if (formats.Distinct().Count() != formats.Count())
            {
                throw new InvalidOperationException("Can't have duplicates in format list");
            }

            // To calculate the mvt we always need a gpkg.
            if (formats.Contains(GeometryExportFormat.Mvt) && !formats.Contains(GeometryExportFormat.Gpkg))
            {
                formats.Add(GeometryExportFormat.Gpkg);
            }

            var bundle = await _bundleRepository.GetByIdAsync(bundleId);
            var layers = await _layerRepository.ListAllFromBundleIdAsync(bundleId).ToListAsync();

            var currentExportedVersion = await _bundleStorageService.GetCurrentExportedVersionAsync(bundle.OrganizationId, bundleId);
            var currentExportedFormats = (currentExportedVersion == null)
                ? new List<GeometryExportFormat>()
                : await _bundleStorageService.GetExportedFormatsAsync(bundle.OrganizationId, bundleId, (uint)currentExportedVersion);

            // Compare the exported formats and the desired formats.
            var missingFormats = formats.Except(currentExportedFormats);

            if (currentExportedVersion == bundle.VersionId)
            {
                // If our exported version is equal to our current version and no
                // formats are missing, we will always be up to date.
                if (!missingFormats.Any())
                {
                    return;
                }

                await ExportFormatsAsync(bundle, layers, missingFormats.ToList());
            }
            else
            {
                await ExportFormatsAsync(bundle, layers, formats.ToList());

                // TODO Do we want to remove any old data?
                // TODO Do we want to schedule this?
                if (currentExportedVersion != null)
                {
                    await _bundleStorageService.CleanupBundleVersionAsync(bundleId, (uint)currentExportedVersion);
                }
            }

            // TODO Clean up? foreach format rm?
        }

        /// <summary>
        ///     This will force a version id refresh and will then call
        ///     <see cref="BuildBundleAsync(Guid, IList{GeometryExportFormat})"/>.
        /// </summary>
        /// <param name="bundleId">The bundle id.</param>
        /// <param name="formats">The desired export formats.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        public async Task ForceBuildBundleAsync(Guid bundleId, IList<GeometryExportFormat> formats)
        {
            await _bundleRepository.ForceUpdateBundleVersionAsync(bundleId);
            await BuildBundleAsync(bundleId, formats);
        }

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
        private async Task ExportFormatsAsync(Bundle bundle, IEnumerable<Layer> layers, IList<GeometryExportFormat> formats)
        {
            // Always start with the .gpkg if we have it
            if (formats.Contains(GeometryExportFormat.Gpkg))
            {
                await ExportAsync(bundle, layers, GeometryExportFormat.Gpkg);
                formats.Remove(GeometryExportFormat.Gpkg);
            }

            foreach (var format in formats)
            {
                await ExportAsync(bundle, layers, format);
            }
        }

        /// <summary>
        ///     Exports a bundle and all its layers to a specified format.
        /// </summary>
        /// <param name="bundle">The bundle to export.</param>
        /// <param name="layers">The bundles layers.</param>
        /// <param name="format">The desired export format.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        private Task ExportAsync(Bundle bundle, IEnumerable<Layer> layers, GeometryExportFormat format)
            => format switch
            {
                GeometryExportFormat.Gpkg => ExportGpkgAsync(bundle, layers.ToList()),
                GeometryExportFormat.Mvt => ExportMvtAsync(bundle, layers),
                _ => throw new InvalidOperationException(nameof(format))
            };

        /// <summary>
        ///     Exports a bundle and all its layers to MVT format.
        /// </summary>
        /// <param name="bundle">The bundle to export.</param>
        /// <param name="layers">Collection of the bundles layers.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        private async Task ExportGpkgAsync(Bundle bundle, IList<Layer> layers)
        {
            var sqlStatements = layers.Select(layer => GetSqlFromLayer(bundle, layer)).ToList();

            // Process the first layer, then append.
            ProcessLayer(layers[0], false);

            // Append the rest of the layers
            for (int i = 0; i < layers.Count; i++)
            {
                ProcessLayer(layers[i], true);
            }

            // When all layers are appended we can upload to storage.
            await _bundleStorageService.UploadExportAsync(bundle.OrganizationId, bundle.Id, bundle.VersionId, GeometryExportFormat.Gpkg);

            // Nested function that exports an sql statement into a GPKG.
            void ProcessLayer(Layer layer, bool append)
            {
                var sql = GetSqlFromLayer(bundle, layer);

                var fileName = $"{BundleNameHelper.GetName(bundle, GeometryExportFormat.Gpkg)}";
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
        /// <param name="layers">Collection of the bundles layers.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        private async Task ExportMvtAsync(Bundle bundle, IEnumerable<Layer> layers)
        {
            var sourceFile = $"{BundleNameHelper.GetName(bundle, GeometryExportFormat.Gpkg)}";
            var targetDirectory = $"{StorageConstants.ExportMvtDirectoryName}";
            var mvtOptions = $"MINZOOM={_options.MvtMinZoom} MAXZOOM={_options.MvtMaxZoom}";

            var commandText = $"ogr2ogr -f MVT {targetDirectory} {sourceFile} {mvtOptions}";

            CommandExecuter.ExecuteCommand(commandText);

            await _bundleStorageService.UploadExportDirectoryAsync(bundle.OrganizationId, bundle.Id, bundle.VersionId, GeometryExportFormat.Mvt);
        }

        /// <summary>
        ///     Builds an SQL statement based on the specified layer.
        /// </summary>
        /// <remarks>
        ///     This also takes the <see cref="Bundle.OrganizationId"/> fence
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

        /// <summary>
        ///     Builds a PG string for the ogr2ogr tool based on our options.
        /// </summary>
        /// <returns>The created PG string.</returns>
        private string BuildPGString()
            => $"PG:\"host={_options.DatabaseHost} dbname={_options.DatabaseName} user={_options.DatabaseUser} password={_options.DatabasePassword}\"";
    }
}
