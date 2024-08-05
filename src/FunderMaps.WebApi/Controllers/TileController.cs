using FunderMaps.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

[AllowAnonymous]
public class TileController : FunderMapsController
{
    [AllowAnonymous]
    [HttpGet("api/tile/{z}/{x}/{y}.vector.pbf")]
    public IActionResult GetAsync(int z, int x, int y)
    {
        var data = new byte[] { 0x00, 0x01, 0x02, 0x03 };

        return File(data, "application/x-protobuf");
    }
}
