using FunderMaps.Core.Controllers;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

[Route("api/metadata")]
public sealed class MetaController(IUserdataRepository userdataRepository) : FunderMapsController
{
    // GET: api/metadata
    [HttpGet]
    public async Task<object> GetAllAsync()
    {
        return await userdataRepository.GetAsync(UserId, "app-0blu4s39"); // TODO: Move to ENV
    }

    // PUT: api/metadata
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] object metadata)
    {
        await userdataRepository.UpdateAsync(UserId, "app-0blu4s39", metadata);

        return NoContent();
    }
}
