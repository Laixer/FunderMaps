using FunderMaps.Core;
using FunderMaps.Core.BackgroundWork;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Types;
using FunderMaps.Infrastructure.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace FunderMaps.Console.BundleServices
{
    /// <summary>
    ///     Contract for handling bundle export storage.
    /// </summary>
    public sealed class BundleStorageService
    {
        private readonly BlobStorageOptions _options;
        private readonly ILogger<BundleStorageService> _logger;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="blobStorageService"></param>
        public BundleStorageService(IOptions<BlobStorageOptions> options,
            ILogger<BundleStorageService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        ///     Uploads a file from our working directory to the correct blob
        ///     storage directory.
        /// </summary>
        /// <remarks>
        ///     This uses the <c>aws s3 cp</c> command.
        ///     This should not be used with formats that export to a directory,
        ///     <see cref="UploadExportDirectory(Guid, Guid, uint, GeometryExportFormat)"/>
        ///     should be used instead.
        /// </remarks>
        /// <param name="organizationId">The bundle organization id.</param>
        /// <param name="bundleId">Bundle id.</param>
        /// <param name="versionId">Bundle version id.</param>
        /// <param name="format">The export format.</param>
        public void UploadExport(Guid organizationId, Guid bundleId, uint versionId, GeometryExportFormat format)
        {
            organizationId.ThrowIfNullOrEmpty();
            bundleId.ThrowIfNullOrEmpty();
            if (format == GeometryExportFormat.Mvt)
            {
                throw new InvalidOperationException($"Use UploadExportDirectoryAsync for folder based export format {format}");
            }

            // TODO Content encoding etc

            var sourceFile = $"{BundleNameHelper.GetName(bundleId, versionId, format)}";
            var targetfile = $"s3://{_options.BlobStorageName}/{StorageConstants.ExportDirectoryName}/{organizationId}/{bundleId}/{versionId}/{FormatExportDirectoryName(format)}";
            var commandText = $"aws s3 cp --endpoint={_options.ServiceUri} {sourceFile} {targetfile}";

            CommandExecuter.ExecuteCommand(commandText);
        }

        /// <summary>
        ///     Uploads a file from our working directory to the correct blob
        ///     storage directory.
        /// </summary>
        /// <remarks>
        ///     This uses the <c>aws s3 cp</c> command.
        /// </remarks>
        /// <param name="organizationId">The bundle organization id.</param>
        /// <param name="bundleId">Bundle id.</param>
        /// <param name="versionId">Bundle version id.</param>
        /// <param name="format">The export format.</param>
        public void UploadExportDirectory(Guid organizationId, Guid bundleId, uint versionId, GeometryExportFormat format)
        {
            organizationId.ThrowIfNullOrEmpty();
            bundleId.ThrowIfNullOrEmpty();
            if (format != GeometryExportFormat.Mvt)
            {
                throw new InvalidOperationException($"Can't use directory export for non-directory format {format}");
            }

            var sourceDirectory = $"{StorageConstants.ExportMvtDirectoryName}";
            var targetDirectory = $"s3://{_options.BlobStorageName}/{StorageConstants.ExportDirectoryName}/{organizationId}/{bundleId}/{versionId}/{FormatExportDirectoryName(format)}/";

            // TODO This contains public read, which is a temporary fix.
            _logger.LogWarning("Note: mvt export contains --acl public-read. This is a tempfix.");

            var formatSpecificOptions = format switch
            {
                GeometryExportFormat.Mvt => "--content-encoding gzip --content-type application/x-protobuf --acl public-read",
                _ => throw new InvalidOperationException(nameof(format))
            };

            var commandText = $"aws s3 cp --endpoint={_options.ServiceUri} {sourceDirectory} {targetDirectory} {formatSpecificOptions} --recursive";

            CommandExecuter.ExecuteCommand(commandText);
        }

        /// <summary>
        ///     Gets the name of the export directory for a given format.
        /// </summary>
        /// <remarks>
        ///     The result does not have a trailing slash.
        /// </remarks>
        /// <param name="format">The requested format.</param>
        /// <returns>The name of the export directory.</returns>
        private static string FormatExportDirectoryName(GeometryExportFormat format)
            => format switch
            {
                GeometryExportFormat.Mvt => StorageConstants.ExportMvtDirectoryName,
                GeometryExportFormat.Gpkg => StorageConstants.ExportGpkgDirectoryName,
                GeometryExportFormat.Shp => StorageConstants.ExportShpDirectoryName,
                GeometryExportFormat.GeoJSON=> StorageConstants.ExportGeoJSONDirectoryName,
                _ => throw new InvalidOperationException(nameof(format))
            };
    }
}
