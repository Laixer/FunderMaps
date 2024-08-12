using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for mapset.
/// </summary>
[AllowAnonymous]
[Route("api/mapset")]
public sealed class MapsetController(IMapsetRepository mapsetRepository) : FunderMapsController
{
    // GET: api/mapset/{name}
    /// <summary>
    ///     Return all mapsets the user has access to or a specific mapset.
    /// </summary>
    [HttpGet("{name?}")]
    [ResponseCache(Duration = 60 * 60, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
    public async Task<IActionResult> GetAsync(string? name)
    {
        var mapSets = new List<Mapset>();

        try
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (name.Trim().StartsWith("cl", StringComparison.CurrentCultureIgnoreCase))
                {
                    mapSets.Add(await mapsetRepository.GetPublicAsync(name));
                }
                else if (name.Trim().StartsWith("ck", StringComparison.CurrentCultureIgnoreCase))
                {
                    mapSets.Add(await mapsetRepository.GetPublicAsync(name));
                }
                else
                {
                    mapSets.Add(await mapsetRepository.GetPublicByNameAsync(name));
                }
            }
        }
        catch (EntityNotFoundException) { }

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
