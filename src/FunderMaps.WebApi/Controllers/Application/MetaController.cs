using FunderMaps.Core.Controllers;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

[Route("api/metadata")]
public sealed class MetaController(IUserdataRepository userdataRepository) : FunderMapsController
{
    // GET: api/metadata
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var rs = await userdataRepository.GetAsync(UserId, "app-0blu4s39"); // TODO: Move to ENV
        return Content(rs.ToString(), "application/json"); // TODO
    }

    // PUT: api/metadata
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] object metadata)
    {
        await userdataRepository.UpdateAsync(UserId, "app-0blu4s39", metadata);

        return NoContent();
    }
}
