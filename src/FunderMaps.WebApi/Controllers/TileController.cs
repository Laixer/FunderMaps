using FunderMaps.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Represents a controller for handling tile requests.
/// </summary>
[AllowAnonymous]
public class TileController : FunderMapsController
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
    public IActionResult GetAsync(string name, int z, int x, int y)
    {
        var data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

        return File(data, "application/x-protobuf");
    }
}
