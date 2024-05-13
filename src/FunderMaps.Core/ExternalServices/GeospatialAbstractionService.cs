using System.Diagnostics;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.ExternalServices;

/// <summary>
///     Geospatial abstraction service.
/// </summary>
internal class GeospatialAbstractionService(ILogger<GeospatialAbstractionService> logger) : IGDALService
{
    private static (string, bool) GetFormat(string input)
    {
        if (input.StartsWith("PG:"))
        {
            return ("PostgreSQL", false);
        }
        else if (input.EndsWith(".gpkg"))
        {
            return ("GPKG", true);
        }
        else if (input.EndsWith(".geojson"))
        {
            return ("GeoJSONSeq", true);
        }

        return ("GPKG", true);
    }

    // TODO: If input is a file, check the file exists. If not throw exception.
    /// <summary>
    ///     Convert geospatial file from one format to another.
    /// </summary>
    /// <param name="input">Input file.</param>
    /// <param name="output">Output file.</param>
    /// <param name="layer">Layer name.</param>
    public void Convert(string input, string output, string? layer = null)
    {
        var (_, isInputFile) = GetFormat(input);
        var (outputFormat, isOutputFile) = GetFormat(output);

        if (isInputFile && !File.Exists(input))
        {
            throw new FileNotFoundException("Input file not found");
        }

        var process = new Process();

        process.StartInfo.FileName = "ogr2ogr";

        process.StartInfo.ArgumentList.Add("-f");
        process.StartInfo.ArgumentList.Add(outputFormat);
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
            logger.LogWarning("Error output: {standardError}", standardError);
        }

        if (!string.IsNullOrEmpty(standardOutput))
        {
            logger.LogInformation("Console output: {standardOutput}", standardOutput);
        }

        if (process.ExitCode != 0)
        {
            // TODO: Add exception type
            throw new InvalidOperationException(standardError);
        }

        if (isOutputFile && !File.Exists(output))
        {
            throw new FileNotFoundException("Output file not found");
        }
    }
}
