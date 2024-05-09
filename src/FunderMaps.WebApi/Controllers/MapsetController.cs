using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for mapset.
/// </summary>
[Route("api/mapset")]
public sealed class MapsetController(IMapsetRepository mapsetRepository) : FunderMapsController
{
    // GET: api/mapset/{id}
    /// <summary>
    ///     Return all mapsets the user has access to.
    /// </summary>
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    [HttpGet("{name}")]
    public async Task<IActionResult> GetAsync(string name)
    {
        var mapSets = new List<Mapset>();

        if (Guid.TryParse(name, out Guid id))
        {
            mapSets.Add(await mapsetRepository.GetPublicAsync(id));
        }
        else if (!string.IsNullOrWhiteSpace(name))
        {
            mapSets.Add(await mapsetRepository.GetPublicByNameAsync(name));
        }

        if (User.Identity?.IsAuthenticated ?? false)
        {
            await foreach (var set in mapsetRepository.GetByOrganizationIdAsync(TenantId))
            {
                mapSets.Add(set);
            }

            // foreach (var organization in _appContext.Organizations)
            // {
            //     await foreach (var set in _mapsetRepository.GetByOrganizationIdAsync2(organization.Id))
            //     {
            //         mapSetList.Add(set);
            //     }
            // }
        }

        return Ok(mapSets);
    }
}
