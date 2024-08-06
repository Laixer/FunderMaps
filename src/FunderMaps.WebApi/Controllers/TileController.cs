using FunderMaps.Core.Controllers;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Represents a controller for handling tile requests.
/// </summary>
[AllowAnonymous]
public class TileController(ITilesetRepository tilesetRepository) : FunderMapsController
{
    /// <summary>
    ///     Retrieves the specified tile data.
    /// </summary>
    /// <param name="name">The name of the tile.</param>
    /// <param name="z">The zoom level of the tile.</param>
    /// <param name="x">The x-coordinate of the tile.</param>
    /// <param name="y">The y-coordinate of the tile.</param>
    /// <returns>The tile data as a file.</returns>
    [HttpGet("api/tile/{name}/{z}/{x}/{y}.vector.pbf")]
    [ResponseCache(Duration = 60 * 60 * 24, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> GetAsync(string name, int z, int x, int y)
    {
        var fileContent = await tilesetRepository.GetTileAsync(name, z, x, y);
        if (fileContent == null || fileContent.Length < 2)
        {
            return NotFound();
        }

        // Check if the file is gzipped.
        if (fileContent[0] == 0x1F && fileContent[1] == 0x8B)
        {
            Response.Headers.Append("Content-Encoding", "gzip");
        }

        return File(fileContent, "application/x-protobuf");
    }
}
