using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application metadata.
/// </summary>
[Route("api/metadata")]
public sealed class MetaController(IUserdataRepository userdataRepository) : FunderMapsController
{
    private const string DefaultApplicationId = "app-0blu4s39";

    // GET: api/metadata
    [HttpGet]
    public async ValueTask<UserData> GetAllAsync()
        => await userdataRepository.GetAsync(UserId, DefaultApplicationId);

    // PUT: api/metadata
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserData userdata)
    {
        await userdataRepository.UpdateAsync(UserId, DefaultApplicationId, userdata);

        return NoContent();
    }
}
