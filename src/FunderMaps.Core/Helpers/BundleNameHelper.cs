using FunderMaps.Core.Entities;
using FunderMaps.Core.Types;
using System;

namespace FunderMaps.Core.Helpers
{
    /// <summary>
    ///     Centralized name generator for bundle exports.
    /// </summary>
    public static class BundleNameHelper
    {
        /// <summary>
        ///     Constructs a filename for a bundle.
        /// </summary>
        /// <param name="bundle">The bundle.</param>
        /// <param name="format">The export format.</param>
        /// <returns>The constructed filename.</returns>
        public static string GetName(Bundle bundle, GeometryExportFormat format)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException(nameof(bundle));
            }

            return GetName(bundle.Id, bundle.VersionId, format);
        }

        // TODO Is this correct?
        /// <summary>
        ///     Constructs a filename for a bundle.
        /// </summary>
        /// <param name="bundleId">The id of the bundle.</param>
        /// <param name="versionId">The version id of the bundle.</param>
        /// <param name="format">The export format.</param>
        /// <returns>The constructed filename.</returns>
        public static string GetName(Guid bundleId, uint versionId, GeometryExportFormat format)
            => format switch
            {
                GeometryExportFormat.Mvt => throw new InvalidOperationException("Can't get file name for a MVT folder structure"),
                GeometryExportFormat.Gpkg => $"{bundleId}-{versionId}.gpkg",
                GeometryExportFormat.Shp => $"{bundleId}-{versionId}.shp",
                GeometryExportFormat.GeoJSON=> $"{bundleId}-{versionId}.geojson",
                _ => throw new InvalidOperationException(nameof(format))
            };
    }
}
