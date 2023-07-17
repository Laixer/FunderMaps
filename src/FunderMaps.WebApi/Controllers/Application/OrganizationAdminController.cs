using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for organization administration.
/// </summary>
/// <remarks>
///     This controller provides organization administration.
///     <para>
///         For the variant based on the current session see 
///         <see cref="FunderMaps.AspNetCore.Controllers.OrganizationController"/>.
///     </para>
/// </remarks>
[Authorize(Policy = "AdministratorPolicy")]
[Route("api/admin/organization")]
public class OrganizationAdminController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IOrganizationRepository _organizationRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OrganizationAdminController(IMapper mapper, IOrganizationRepository organizationRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
    }

    // GET: api/admin/organization/stats
    /// <summary>
    ///     Return organization statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        // Map.
        var output = new DatasetStatsDto()
        {
            Count = await _organizationRepository.CountAsync(),
        };

        // Return.
        return Ok(output);
    }

    // GET: api/admin/organization/{id}
    /// <summary>
    ///     Return organization by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    public Task<Organization> GetAsync(Guid id)
        => _organizationRepository.GetByIdAsync(id);

    // GET: api/admin/organization
    /// <summary>
    ///     Return all organizations.
    /// </summary>
    [HttpGet]
    public IAsyncEnumerable<Organization> GetAllAsync([FromQuery] PaginationDto pagination)
        => _organizationRepository.ListAllAsync(pagination.Navigation);

    // PUT: api/admin/organization/{id}
    /// <summary>
    ///     Update organization by id.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Organization input)
    {
        // Map.
        var organization = _mapper.Map<Organization>(input);
        organization.Id = id;

        // Act.
        await _organizationRepository.UpdateAsync(organization);

        // Return.
        return NoContent();
    }

    // DELETE: api/admin/organization/{id}
    /// <summary>
    ///     Delete organization by id.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        // Act.
        await _organizationRepository.DeleteAsync(id);

        // Return.
        return NoContent();
    }
}
