using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application metadata.
/// </summary>
/// <param name="userdataRepository"></param>
[Route("api/metadata")]
public sealed class MetaController(IUserdataRepository userdataRepository) : FunderMapsController
{
    // GET: api/metadata
    [HttpGet]
    public async ValueTask<UserData> GetAllAsync()
        => await userdataRepository.GetAsync(UserId, "app-0blu4s39"); // TODO: Move to ENV

    // PUT: api/metadata
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserData userdata)
    {
        await userdataRepository.UpdateAsync(UserId, "app-0blu4s39", userdata);

        return NoContent();
    }
}
