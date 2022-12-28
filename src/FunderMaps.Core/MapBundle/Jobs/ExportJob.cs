using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Threading.Command;
using FunderMaps.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.MapBundle.Jobs;

/// <summary>
///     Bundle job entry.
/// </summary>
public class ExportJob : CommandTask
{
    private const string TaskName = "BUNDLE_BUILDING";

    private static readonly string[] exportLayers = new[]
    {
            "analysis_building",
            "analysis_foundation",
            "analysis_quality",
            "analysis_report",
            "incident",
            "incident_aggregate",
            "incident_aggregate_category",
        };

    private readonly string connectionString;

    private readonly IMapService _mapService;
    private readonly IBlobStorageService _blobStorageService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ExportJob(IConfiguration configuration, IMapService mapService, IBlobStorageService blobStorageService, ILogger<ExportJob> logger)
    {
        Logger = logger;
        connectionString = configuration.GetConnectionString("FunderMapsConnection");
        _mapService = mapService;
        _blobStorageService = blobStorageService;
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

        // Remove any previous artefacts.
        if (Directory.Exists(context.Workspace))
        {
            Directory.Delete(context.Workspace, true);
        }

        // var layer = exportLayers.OrderBy(i => rng.Next()).First();
        foreach (var layer in exportLayers.OrderBy(i => rng.Next()))
        {
            Logger.LogInformation($"Building layer {layer}");

            async Task<FileDataSource> buildGpkg()
            {
                FileDataSource fileOutput = new()
                {
                    Format = GeometryFormat.GeoPackage,
                    PathPrefix = CreateDirectory("out"),
                    Extension = ".gpkg",
                    Name = layer,
                };

                CommandInfo command = new VectorDatasetBuilder()
                    .InputDataset(new PostreSQLDataSource(connectionString))
                    .InputLayers(new BundleLayerSource(layer, Context.Workspace))
                    .OutputDataset(fileOutput)
                    .Build(formatName: "GPKG");

                if (await RunCommandAsync(command) == 0)
                {
                    Logger.LogDebug("GPKG dataset complete");

                    using var fileOutputStream = File.OpenRead(fileOutput.ToString());

                    await _blobStorageService.StoreFileAsync(
                        containerName: "geo-bundle",
                        fileName: fileOutput.FileName,
                        contentType: "application/x-sqlite3",
                        stream: fileOutputStream);

                    Logger.LogDebug("GPKG upload complete");

                    return fileOutput;
                }

                throw new Exception();
            }

            var bundleFileGpkg = await buildGpkg();

            async Task<FileDataSource> buildGeoJSON(FileDataSource fileInput)
            {
                FileDataSource fileOutput = new()
                {
                    Format = GeometryFormat.GeoJSONSeq,
                    PathPrefix = CreateDirectory("out"),
                    Extension = ".json",
                    Name = layer,
                };

                CommandInfo command = new VectorDatasetBuilder()
                    .InputDataset(fileInput)
                    .OutputDataset(fileOutput)
                    .Build(formatName: "GeoJSONSeq");

                if (await RunCommandAsync(command) == 0)
                {
                    Logger.LogDebug("GeoJSON dataset complete");

                    using var fileOutputStream = File.OpenRead(fileOutput.ToString());

                    await _blobStorageService.StoreFileAsync(
                        containerName: "geo-bundle",
                        fileName: fileOutput.FileName,
                        contentType: "application/json",
                        stream: fileOutputStream);

                    Logger.LogDebug("GeoJSON upload complete");

                    return fileOutput;
                }

                throw new Exception();
            }

            var bundleFileGeoJSON = await buildGeoJSON(bundleFileGpkg);

            // NOTE: We *must* delete the old dataset first.
            await _mapService.DeleteDatasetAsync(layer);

            await _mapService.UploadDatasetAsync(layer, bundleFileGeoJSON.ToString());

            if (await _mapService.PublishAsync(layer))
            {
                Logger.LogInformation($"Layer published");
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
