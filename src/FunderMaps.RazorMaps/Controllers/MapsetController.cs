using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.RazorMaps.Controllers;

/// <summary>
///     Controller for all product endpoints.
/// </summary>
[Route("mapset/{id:guid?}")]
public class MapsetController : ControllerBase
{
    private readonly IMapsetRepository _mapsetRepository;
    private readonly Core.AppContext _appContext;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public MapsetController(IMapsetRepository mapsetRepository, Core.AppContext appContext)
    {
        _mapsetRepository = mapsetRepository;
        _appContext = appContext;
    }

    // GET: mapset
    /// <summary>
    ///     Request the analysis product.
    /// </summary>
    // [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        List<Core.Entities.Mapset> mapSetList = new();

        if (id != Guid.Empty)
        {
            var set = await _mapsetRepository.GetPublicAsync2(id);
            mapSetList.Add(set);
        }

        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            foreach (var organization in _appContext.Organizations)
            {
                await foreach (var set in _mapsetRepository.GetByOrganizationIdAsync2(organization.Id))
                {
                    mapSetList.Add(set);
                }
            }
        }

        return Ok(mapSetList);
    }
}
