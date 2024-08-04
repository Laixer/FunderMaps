using FunderMaps.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

[Route("api")]
public sealed class MetaController : FunderMapsController
{
    // GET: api/metadata
    [HttpGet("metadata")]
    public async Task<object> GetAllAsync()
    {
        await Task.CompletedTask;

        // TODO: Retrieve metadata from user+tenant context.

        return new { };
    }

    // PUT: api/metadata
    [HttpPut("metadata")]
    public async Task<object> UpdateAsync()
    {
        await Task.CompletedTask;

        // TODO: Store metadata in user+tenant context.

        return new { };
    }
}
