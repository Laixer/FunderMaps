using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Model;

[Route("api/subsidence")]
public sealed class SubsidenceController(ISubsidenceRepository subsidenceRepository) : FunderMapsController
{
    [HttpGet("{id}"), ResponseCache(Duration = 60 * 60 * 12)]
    public async ValueTask<List<SubsidenceHistory>> GetAllAsync(string id)
    {
        return await subsidenceRepository.ListAllHistoryByIdAsync(id).ToListAsync();
    }
}
