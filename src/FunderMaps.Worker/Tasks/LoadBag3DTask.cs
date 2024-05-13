using FunderMaps.Core.Helpers;
using FunderMaps.Core.Interfaces;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Data.Providers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FunderMaps.Worker.Tasks;

/// <summary>
///     Construct new instance.
/// </summary>
internal sealed class LoadBag3DTask(
    IOptions<DbProviderOptions> dbProviderOptions,
    IGDALService gdalService,
    IOperationRepository operationRepository,
    ILogger<LoadBag3DTask> logger) : ITaskService
{
    private const int MinimumFileSize = 1024 * 1024; // 1 MB

    private readonly DbProviderOptions _dbProviderOptions = dbProviderOptions?.Value ?? throw new ArgumentNullException(nameof(dbProviderOptions));

    /// <summary>
    ///    Triggered when the application host is ready to start the service.
    /// </summary>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");

            var fileName = "/home/yorick/Projects/FunderMaps/data/3DBAG/export/3dbag_nl.gpkg";

            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("Downloaded BAG file not found", fileInfo.Name);
            }

            if (fileInfo.Length < MinimumFileSize)
            {
                throw new InvalidOperationException("Downloaded BAG file is too small");
            }

            logger.LogInformation("Processing BAG 3D file");

            var dataSourceBuilder = new Npgsql.NpgsqlConnectionStringBuilder(_dbProviderOptions.ConnectionString);
            var output = $"PG:dbname='{dataSourceBuilder.Database}' host='{dataSourceBuilder.Host}' port='{dataSourceBuilder.Port}' user='{dataSourceBuilder.Username}' password='{dataSourceBuilder.Password}'";
            gdalService.Convert(fileName, output, "lod22_2d");
            gdalService.Convert(fileName, output, "pand");
        }
        finally
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg");
            FileHelper.DeleteFilesWithExtension(currentDirectory, "gpkg-journal");

            await operationRepository.CleanupBAGAsync();
        }
    }
}
