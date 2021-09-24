using System.Threading.Tasks;
using FunderMaps.Core.Types;
using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FunderMaps.Core.MapBundle.Jobs
{
    /// <summary>
    ///     Bundle job entry.
    /// </summary>
    internal class ExportJob : CommandTask
    {
        private const string TaskName = "BUNDLE_BUILDING";

        private static readonly string[] exportLayers = new string[]
        {
            "analysis_building",
            "analysis_foundation",
            "analysis_quality",
            "analysis_report",
        };

        private string connectionString;

        private readonly IMapService _mapService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ExportJob(IConfiguration configuration, IMapService mapService)
        {
            connectionString = configuration.GetConnectionString("FunderMapsConnection");
            _mapService = mapService;
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
            const int features_per_part = 1000000;

            for (int i = 0; i < 10; i++)
            {
                FileDataSource fileDump = new()
                {
                    Format = GeometryFormat.GeoJSONSeq,
                    PathPrefix = CreateDirectory("out"),
                    Extension = ".json",
                    Name = $"{layer}_part{i}",
                };

                CommandInfo command = new VectorDatasetBuilder()
                    .InputDataset(new PostreSQLDataSource(connectionString))
                    .InputLayers(new BundleLayerSource(layer, Context.Workspace, i * features_per_part, features_per_part))
                    .OutputDataset(fileDump)
                    .Build(formatName: "GeoJSONSeq");

                Logger.LogDebug($"Export layer: {layer}, part: {i}");

                if (await RunCommandAsync(command) == 0)
                {
                    long length = new FileInfo(fileDump.ToString()).Length;

                    if (length > 0)
                    {
                        bool success = await _mapService.UploadDatasetAsync(layer, fileDump.ToString());

                        Logger.LogDebug($"Upload layer: {layer}, part: {i}, success: {success}");
                    }

                    File.Delete(fileDump.ToString());

                    if (length == 0)
                    {
                        return true;
                    }
                }
                else
                {
                    Logger.LogError($"Export layer: {layer}, part: {i} failed");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            foreach (var layer in exportLayers)
            {
                await _mapService.DeleteDatasetAsync(layer);

                if (await ExportDatasetAsync(layer))
                {
                    if (await _mapService.PublishAsync(layer))
                    {
                        Logger.LogInformation($"Layer {layer} published with success");
                    }
                    else
                    {
                        Logger.LogError($"Layer {layer} was not published");
                    }
                }
            }
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
