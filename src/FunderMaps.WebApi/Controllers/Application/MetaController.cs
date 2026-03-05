using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application metadata.
/// </summary>
[Route("api/metadata")]
public sealed class MetaController(IUserdataRepository userdataRepository, IConfiguration configuration) : FunderMapsController
{
    private string ApplicationId => configuration["Application:Id"] ?? throw new InvalidOperationException("Application:Id not found in configuration.");

    // GET: api/metadata
    [HttpGet]
    public async ValueTask<UserData> GetAllAsync()
        => await userdataRepository.GetAsync(UserId, ApplicationId);

    // PUT: api/metadata
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserData userdata)
    {
        await userdataRepository.UpdateAsync(UserId, ApplicationId, userdata);

        return NoContent();
    }
}
