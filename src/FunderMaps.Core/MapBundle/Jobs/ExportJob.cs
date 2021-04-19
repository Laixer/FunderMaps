using System.Threading.Tasks;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using FunderMaps.Core.Threading.Command;
using Microsoft.Extensions.Logging;
using FunderMaps.Core.Interfaces;

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
        ///     Run the background command.
        /// </summary>
        /// <param name="context">Command task execution context.</param>
        public override async Task ExecuteCommandAsync(CommandTaskContext context)
        {
            foreach (var layer in exportLayers)
            {
                FileDataSource fileDump = new()
                {
                    Format = GeometryFormat.GeoJSONSeq,
                    PathPrefix = CreateDirectory("out"),
                    Extension = ".json",
                    Name = layer,
                };

                CommandInfo command = new VectorDatasetBuilder()
                    .InputDataset(new PostreSQLDataSource(connectionString))
                    .InputLayers(new BundleLayerSource(layer, Context.Workspace))
                    .OutputDataset(fileDump)
                    .Build(formatName: "GeoJSONSeq");

                if (await RunCommandAsync(command) == 0)
                {
                    bool success = await _mapService.UploadDatasetAsync(layer, fileDump.ToString());

                    Logger.LogInformation($"Upload layer: {layer}, success: {success}");

                    success = await _mapService.PublishAsync(layer);

                    Logger.LogInformation($"Pulish layer: {layer}, success: {success}");
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
