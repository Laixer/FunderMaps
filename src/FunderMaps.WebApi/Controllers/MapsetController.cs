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
    // TODO: This method should accept more than GUID. The id could also be a string.
    // GET: api/mapset/{id}
    /// <summary>
    ///     Return all mapsets the user has access to.
    /// </summary>
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    [HttpGet("{id:guid?}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var mapSets = new List<Mapset>();

        // TODO: TryParse
        if (id != Guid.Empty)
        {
            mapSets.Add(await mapsetRepository.GetPublicAsync(id));
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
