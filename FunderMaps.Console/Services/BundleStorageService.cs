using FunderMaps.Core;
using FunderMaps.Core.Extensions;
using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Types;
using FunderMaps.Infrastructure.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Console.Services
{
    /// <summary>
    ///     Contract for handling bundle export storage.
    /// </summary>
    public sealed class BundleStorageService
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly BlobStorageOptions _options;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        /// <param name="blobStorageService"></param>
        public BundleStorageService(IBlobStorageService blobStorageService,
            IOptions<BlobStorageOptions> options)
        {
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        ///     Gets the current stored bundle version from our blob storage.
        /// </summary>
        /// <param name="organizationId">The bundles organization id.</param>
        /// <param name="bundleId">The bundle id.</param>
        /// <returns>The current bundle version id.</returns>
        public async Task<uint?> GetCurrentExportedVersionAsync(Guid organizationId, Guid bundleId)
        {
            organizationId.ThrowIfNullOrEmpty();
            bundleId.ThrowIfNullOrEmpty();

            var subdirs = await _blobStorageService.ListSubcontainerNamesAsync(BundleContainer(organizationId, bundleId));
            if (!subdirs.Any())
            {
                // TODO Is this a good idea?
                return null;
            }

            var ordered = subdirs.Select(x => uint.Parse(x)).ToList();
            ordered.Sort();

            // TODO Or last
            return ordered.First();
        }

        /// <summary>
        ///     Gets all exported formats for a given version id of a bundle.
        /// </summary>
        /// <param name="bundleId">The bundle id.</param>
        /// <param name="versionId">The bundle version id.</param>
        /// <returns>Collection of all exported formats.</returns>
        public async Task<IEnumerable<GeometryExportFormat>> GetExportedFormatsAsync(Guid organizationId, Guid bundleId, uint versionId)
        {
            organizationId.ThrowIfNullOrEmpty();
            bundleId.ThrowIfNullOrEmpty();
            // TODO Check version id?

            var container = $"{BundleContainer(organizationId, bundleId)}/{versionId}";
            var subdirs = await _blobStorageService.ListSubcontainerNamesAsync(container);

            var result = new List<GeometryExportFormat>();
            
            foreach (var subdir in subdirs)
            {
                switch (subdir)
                {
                    case StorageConstants.ExportGpkgDirectoryName:
                        result.Add(GeometryExportFormat.Gpkg);
                        break;
                    case StorageConstants.ExportMvtDirectoryName:
                        result.Add(GeometryExportFormat.Mvt);
                        break;

                    // TODO Append other formats here!
                }
            }

            return result;
        }

        // TODO Do we even want this?
        /// <summary>
        ///     Gets rid of all files in a bundles version.
        /// </summary>
        /// <param name="bundleId">The bundle id.</param>
        /// <param name="versionId">The bundle version id.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        public Task CleanupBundleVersionAsync(Guid bundleId, uint versionId)
            => throw new NotImplementedException();

        /// <summary>
        ///     Uploads a file from our working directory to the correct blob
        ///     storage directory.
        /// </summary>
        /// <remarks>
        ///     This uses the <c>aws s3 cp</c> command.
        ///     This should not be used with formats that export to a directory,
        ///     <see cref="UploadExportDirectoryAsync(Guid, Guid, uint, GeometryExportFormat)"/>
        ///     should be used instead.
        /// </remarks>
        /// <param name="organizationId">The bundle organization id.</param>
        /// <param name="bundleId">Bundle id.</param>
        /// <param name="versionId">Bundle version id.</param>
        /// <param name="format">The export format.</param>
        /// <returns>See <see cref="Task"/>.</returns>
        public Task UploadExportAsync(Guid organizationId, Guid bundleId, uint versionId, GeometryExportFormat format)
        {
            organizationId.ThrowIfNullOrEmpty();
            bundleId.ThrowIfNullOrEmpty();
            // TODO Check version?
            if (format == GeometryExportFormat.Mvt)
            {
                throw new InvalidOperationException($"Use UploadExportDirectoryAsync for folder based export format {format}");
            }

            var sourceFile = $"{BundleNameHelper.GetName(bundleId, versionId, format)}";
            var targetfile = $"s3://{_options.BlobStorageName}/{StorageConstants.ExportDirectoryName}/{organizationId}/{bundleId}/{versionId}/{FormatExportDirectoryName(format)}";
            var commandText = $"aws s3 cp --endpoint={_options.ServiceUri} {sourceFile} {targetfile}";

            CommandExecuter.ExecuteCommand(commandText);

            // TODO Needs to be async?
            return Task.CompletedTask;
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
        /// <returns>See <see cref="Task"/>.</returns>
        public Task UploadExportDirectoryAsync(Guid organizationId, Guid bundleId, uint versionId, GeometryExportFormat format)
        {
            organizationId.ThrowIfNullOrEmpty();
            bundleId.ThrowIfNullOrEmpty();
            // TODO Check version?
            if (format != GeometryExportFormat.Mvt)
            {
                throw new InvalidOperationException($"Can't use directory export for non-directory format {format}");
            }

            var sourceDirectory = $"{StorageConstants.ExportMvtDirectoryName}";
            var targetDirectory = $"s3://{_options.BlobStorageName}/{StorageConstants.ExportDirectoryName}/{organizationId}/{bundleId}/{versionId}/{FormatExportDirectoryName(format)}/";

            // TODO This contains public read, which is a temporary fix.
            System.Console.WriteLine("Note: mvt export contains --acl public-read. This is a tempfix.");

            var formatSpecificOptions = format switch
            {
                GeometryExportFormat.Mvt => "--content-encoding gzip --content-type application/x-protobuf --acl public-read",
                _ => throw new InvalidOperationException(nameof(format))
            };

            var commandText = $"aws s3 cp --endpoint={_options.ServiceUri} {sourceDirectory} {targetDirectory} {formatSpecificOptions} --recursive";

            CommandExecuter.ExecuteCommand(commandText);

            // TODO Need async?
            return Task.CompletedTask;
        }

        /// <summary>ll
        ///     Builds the root container for a given bundle.
        /// </summary>
        /// <remarks>
        ///     This has a trailing slash / at the end.
        /// </remarks>
        /// <param name="organizationId">Bundle organization id.</param>
        /// <param name="bundleId">Bundle id.</param>
        /// <returns>Root container path.</returns>
        private static string BundleContainer(Guid organizationId, Guid bundleId)
            => $"{StorageConstants.ExportDirectoryName}/{organizationId}/{bundleId}/";

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
                _ => throw new InvalidOperationException(nameof(format))
            };
    }
}
