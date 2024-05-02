using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Report;

/// <summary>
///     Endpoint controller for incident operations.
/// </summary>
/// <remarks>
///     Create new instance.
/// </remarks>
[Route("api/incident")]
public sealed class IncidentController(
    IIncidentRepository incidentRepository,
    GeocoderTranslation geocoderTranslation) : FunderMapsController
{
    // GET: api/incident/stats
    /// <summary>
    ///     Return incident statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        var output = new DatasetStatsDto()
        {
            Count = await incidentRepository.CountAsync(),
        };

        return Ok(output);
    }

    // GET: api/incident/{id}
    /// <summary>
    ///     Return incident by id.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<Incident> GetAsync(string id)
        => await incidentRepository.GetByIdAsync(id);

    // GET: api/incident/building/{id}
    /// <summary>
    ///    Return all incidents for a building by identifier.
    /// </summary>
    [HttpGet("building/{id}")]
    public async Task<IEnumerable<Incident>> GetAllByBuildingIdAsync(string id, [FromQuery] PaginationDto pagination)
    {
        var building = await geocoderTranslation.GetBuildingIdAsync(id);
        return await incidentRepository.ListAllByBuildingIdAsync(building.Id).ToListAsync();
    }

    // GET: api/incident
    /// <summary>
    ///     Return all incidents.
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<Incident>> GetAllAsync([FromQuery] PaginationDto pagination)
        => await incidentRepository.ListAllAsync(pagination.Navigation).ToListAsync();

    // POST: api/incident
    /// <summary>
    ///     Create incident.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<Incident> CreateAsync([FromBody] Incident incident)
    {
        incident.Id = await incidentRepository.AddAsync(incident);

        return await incidentRepository.GetByIdAsync(incident.Id);
    }

    // PUT: api/incident/{id}
    /// <summary>
    ///     Update incident by id.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "WriterAdministratorPolicy")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Incident incident)
    {
        incident.Id = id;

        await incidentRepository.UpdateAsync(incident);

        return NoContent();
    }

    // DELETE: api/incident/{id}
    /// <summary>
    ///     Delete incident by id.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "SuperuserAdministratorPolicy")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await incidentRepository.DeleteAsync(id);

        return NoContent();
    }
}
