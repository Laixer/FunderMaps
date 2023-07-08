using System.Diagnostics;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.Services;

/// <summary>
///     Geospatial abstraction service.
/// </summary>
internal class GeospatialAbstractionService : IGDALService
{
    private readonly ILogger<GeospatialAbstractionService> _logger;

    /// <summary>
    ///     Construct new instance.
    /// </summary>
    public GeospatialAbstractionService(ILogger<GeospatialAbstractionService> logger)
        => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    ///     Convert geospatial file from one format to another.
    /// </summary>
    /// <param name="input">Input file.</param>
    /// <param name="output">Output file.</param>
    /// <param name="layer">Layer name.</param>
    public void Convert(string input, string output, string? layer = null)
    {
        var format = "GPKG";
        if (output.StartsWith("PG:"))
        {
            format = "PostgreSQL";
        }
        else if (output.EndsWith(".gpkg"))
        {
            format = "GPKG";
        }
        else if (output.EndsWith(".geojson"))
        {
            format = "GeoJSONSeq";
        }

        var process = new Process();

        process.StartInfo.FileName = "ogr2ogr";

        process.StartInfo.ArgumentList.Add("-f");
        process.StartInfo.ArgumentList.Add(format);
        process.StartInfo.ArgumentList.Add(output);
        process.StartInfo.ArgumentList.Add(input);

        if (!string.IsNullOrEmpty(layer))
        {
            process.StartInfo.ArgumentList.Add(layer);
        }

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        string standardError = process.StandardError.ReadToEnd();
        string standardOutput = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(standardError))
        {
            _logger.LogError("Error output: " + standardError);
        }

        if (!string.IsNullOrEmpty(standardOutput))
        {
            _logger.LogInformation("Console output: " + standardOutput);
        }
    }
}
