using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application contractors.
/// </summary>
public class ContractorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IContractorRepository _contractorRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ContractorController(IMapper mapper, IOrganizationRepository organizationRepository, IContractorRepository contractorRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
        _contractorRepository = contractorRepository ?? throw new ArgumentNullException(nameof(contractorRepository));
    }

    // GET: api/contractor
    /// <summary>
    ///     Return all contractors.
    /// </summary>
    /// <remarks>
    ///     Cache response for 8 hours. Contractors will not change often.
    ///     Contractors are tenant independent.
    /// </remarks>
    // [HttpGet("contractor"), ResponseCache(Duration = 60 * 60 * 8)]
    [HttpGet("contractor")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
    {
        // Assign.
        IAsyncEnumerable<Organization> organizationList = _organizationRepository.ListAllAsync(pagination.Navigation);

        IAsyncEnumerable<Contractor> contractorList = _contractorRepository.ListAllAsync(pagination.Navigation);

        return Ok(contractorList);
    }
}
