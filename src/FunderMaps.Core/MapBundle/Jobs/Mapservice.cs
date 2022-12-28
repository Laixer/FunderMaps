using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Threading.Command;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.MapBundle.Jobs
{
    /// <summary>
    ///     Bundle job entry.
    /// </summary>
    public class Mapservice : CommandTask
    {
        private const string TaskName = "BUNDLE_BUILDING";

        private readonly IAnalysisRepository _analysisRepository;
        private readonly IMapService _mapService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public Mapservice(IAnalysisRepository analysisRepository, IMapService mapService, ILogger<Mapservice> logger)
        {
            Logger = logger;
            _analysisRepository = analysisRepository;
            _mapService = mapService;
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
            // await _mapService.UploadDatasetToTilesetAsync("cl7rez7l11lly21o0sl9x8bzd", "analysis_static");

            // await _mapService.UploadStatusAsync("cl7t0id5d18gd27pcvc21bthk");

            // await foreach (var x in _analysisRepository.ListAllAsync(Navigation.All))
            // {
            //     Logger.LogInformation($"Patch feature: {x.BuildingId}");

            //     for (int i = 0; i < 5; i++)
            //     {
            //         try
            //         {
            //             await _mapService.UploadDatasetFeatureAsync("cl7rez7l11lly21o0sl9x8bzd", x.BuildingId, x.GeoJson.ToString());
            //             break;
            //         }
            //         catch (System.Exception)
            //         {
            //             if (i == 3)
            //             {
            //                 Logger.LogCritical("Failed after 3 attempts");
            //                 Environment.Exit(1);
            //             }
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
