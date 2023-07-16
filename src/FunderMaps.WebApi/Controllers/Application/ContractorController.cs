using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application contractors.
/// </summary>
[Route("api")]
public class ContractorController : ControllerBase
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
    public IAsyncEnumerable<Contractor> GetAllAsync([FromQuery] PaginationDto pagination, [FromServices] IContractorRepository contractorRepository)
        => contractorRepository.ListAllAsync(Navigation.All);
}
