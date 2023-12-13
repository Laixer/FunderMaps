using System.Diagnostics;
using FunderMaps.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunderMaps.Core.ExternalServices;

/// <summary>
///     Geospatial abstraction service.
/// </summary>
internal class TippecanoeService(ILogger<TippecanoeService> logger) : ITilesetGeneratorService
{
    /// <summary>
    ///     Generate vector tiles from input.
    /// </summary>
    public void Generate(string input, string output, string? layer = null, int maxZoomLevel = 15, int minZoomLevel = 10, CancellationToken cancellationToken = default)
    {
        var process = new Process();

        process.StartInfo.FileName = "tippecanoe";

        process.StartInfo.ArgumentList.Add("--force");
        process.StartInfo.ArgumentList.Add("--read-parallel");
        process.StartInfo.ArgumentList.Add("-z");
        process.StartInfo.ArgumentList.Add(maxZoomLevel.ToString());
        process.StartInfo.ArgumentList.Add("-Z");
        process.StartInfo.ArgumentList.Add(minZoomLevel.ToString());
        process.StartInfo.ArgumentList.Add("-o");
        process.StartInfo.ArgumentList.Add(output);
        process.StartInfo.ArgumentList.Add("--drop-densest-as-needed");

        if (!string.IsNullOrEmpty(layer))
        {
            process.StartInfo.ArgumentList.Add("-l");
            process.StartInfo.ArgumentList.Add(layer);
        }

        process.StartInfo.ArgumentList.Add(input);

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;

        process.Start();

        cancellationToken.Register(() =>
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
        });

        string standardError = process.StandardError.ReadToEnd();
        string standardOutput = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(standardError))
        {
            logger.LogError("Error output: {standardError}", standardError);
        }

        if (!string.IsNullOrEmpty(standardOutput))
        {
            logger.LogInformation("Console output: {standardOutput}", standardOutput);
        }
    }

    /// <summary>
    ///     Test the Tippcanoe service.
    /// </summary>
    public Task HealthCheck()
    {
        var processStartInfo = new ProcessStartInfo()
        {
            FileName = "tippecanoe",
            Arguments = "--version"
        };

        using var process = Process.Start(processStartInfo) ?? throw new InvalidOperationException("Tippecanoe is not installed.");
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException("Tippecanoe is not installed.");
        }

        return Task.CompletedTask;
    }
}
