using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.MapBundle.Jobs
{
    /// <summary>
    ///     Bundle job entry.
    /// </summary>
    public class ExportGpkg : CommandTask
    {
        private const string TaskName = "BUNDLE_BUILDING";

        private static readonly string[] exportLayers = new[]
        {
            "analysis_building",
        };

        private readonly string connectionString;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ExportGpkg(IConfiguration configuration, ILogger<ExportJob> logger)
        {
            Logger = logger;
            connectionString = configuration.GetConnectionString("FunderMapsConnection");
        }

        /// <summary>
        ///     Export dataset from database to mapservice.
        /// </summary>
        /// <remarks>
        ///     The export is performed in 10 parts to that each individual part is
        ///     less likely to fail the upload, and does not claim to much disk space.
        ///     A tileset source can be composed of up to 10 source files so this number
        ///     should not be increased.
        /// </remarks>
        /// <param name="layer">Layer name.</param>
        /// <returns><c>True</c> if the export and upload succeeded, false otherwise.</returns>
        private async Task<bool> ExportDatasetAsync(string layer)
        {
            FileDataSource fileDump = new()
            {
                Format = GeometryFormat.GeoPackage,
                PathPrefix = CreateDirectory("out"),
                Extension = ".gpkg",
                Name = layer,
            };

            CommandInfo command = new VectorDatasetBuilder()
                .InputDataset(new PostreSQLDataSource(connectionString))
                .InputLayers(new BundleLayerSource(layer, Context.Workspace, 0, 100_000_000))
                .OutputDataset(fileDump)
                .Build(formatName: "GPKG");

            await RunCommandAsync(command);

            // Logger.LogDebug($"Export layer: {layer}, part: {i}");

            // if (await RunCommandAsync(command) == 0)
            // {
            //     long length = new FileInfo(fileDump.ToString()).Length;

            //     if (length > 0)
            //     {
            //         bool success = await _mapService.UploadDatasetAsync(layer, fileDump.ToString());

            //         Logger.LogDebug($"Upload layer: {layer}, part: {i}, success: {success}");
            //     }

            //     File.Delete(fileDump.ToString());

            //     if (length == 0)
            //     {
            //         return true;
            //     }
            // }
            // else
            // {
            //     Logger.LogError($"Export layer: {layer}, part: {i} failed");
            //     return false;
            // }

            return true;
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <remarks>
        ///     Select the export layers in random order.
        /// </remarks>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            Context = context;
            var rng = new Random();

            await ExportDatasetAsync(exportLayers[0]);

            // var layer = exportLayers.OrderBy(i => rng.Next()).First();
            // foreach (var layer in exportLayers.OrderBy(i => rng.Next()))
            // {
            //     // NOTE: We *must* delete the old dataset first.
            //     await _mapService.DeleteDatasetAsync(layer);

            //     if (await ExportDatasetAsync(layer))
            //     {
            //         if (await _mapService.PublishAsync(layer))
            //         {
            //             Logger.LogInformation($"Layer {layer} published with success");
            //         }
            //         else
            //         {
            //             Logger.LogError($"Layer {layer} was not published");
            //         }
            //     }
            // }
        }

        /// <summary>
        ///     Method to check if a task can be handeld by this job.
        /// </summary>
        /// <param name="name">The task name.</param>
        /// <param name="value">The task payload.</param>
        /// <returns><c>True</c> if method handles task, false otherwise.</returns>
        public override bool CanHandle(string name, object value)
            => name is not null && name.ToUpperInvariant() == TaskName;
    }
}
