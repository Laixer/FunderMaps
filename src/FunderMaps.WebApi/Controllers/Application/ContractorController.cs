using FunderMaps.Core;
using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application contractors.
/// </summary>
[Route("api")]
public sealed class ContractorController(IContractorRepository contractorRepository) : FunderMapsController
{
    // GET: api/contractor
    /// <summary>
    ///     Return all contractors.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Contractors will not change often.
    ///     Contractors are tenant independent.
    /// </remarks>
    [HttpGet("contractor"), ResponseCache(Duration = 60 * 60 * 12)]
    public async Task<IEnumerable<Contractor>> GetAllAsync([FromQuery] PaginationDto pagination)
        => await contractorRepository.ListAllAsync(Navigation.All).ToListAsync();
}
