using FunderMaps.Core.Controllers;
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
    /// <summary>
    ///     Return all metadata.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        // TODO: Create a UserData entity
        var metadata = await userdataRepository.GetAsync(UserId, "app-0blu4s39"); // TODO: Move to ENV
        return Content(metadata.ToString() ?? "{}", "application/json");
    }

    // PUT: api/metadata
    /// <summary>
    ///     Update metadata.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] object metadata)
    {
        await userdataRepository.UpdateAsync(UserId, "app-0blu4s39", metadata);

        return NoContent();
    }
}
