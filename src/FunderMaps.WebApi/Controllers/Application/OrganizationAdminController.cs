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
///         <see cref="AspNetCore.Controllers.OrganizationController"/>.
///     </para>
/// </remarks>
/// <remarks>
///     Create new instance.
/// </remarks>
[Authorize(Policy = "AdministratorPolicy")]
[Route("api/admin/organization")]
public class OrganizationAdminController(IOrganizationRepository organizationRepository) : ControllerBase
{
    // GET: api/admin/organization/stats
    /// <summary>
    ///     Return organization statistics.
    /// </summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStatsAsync()
    {
        var output = new DatasetStatsDto()
        {
            Count = await organizationRepository.CountAsync(),
        };

        return Ok(output);
    }

    // GET: api/admin/organization/{id}
    /// <summary>
    ///     Return organization by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    public Task<Organization> GetAsync(Guid id)
        => organizationRepository.GetByIdAsync(id);

    // GET: api/admin/organization
    /// <summary>
    ///     Return all organizations.
    /// </summary>
    [HttpGet]
    public IAsyncEnumerable<Organization> GetAllAsync([FromQuery] PaginationDto pagination)
        => organizationRepository.ListAllAsync(pagination.Navigation);

    // PUT: api/admin/organization/{id}
    /// <summary>
    ///     Update organization by id.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Organization organization)
    {
        organization.Id = id;

        await organizationRepository.UpdateAsync(organization);

        return NoContent();
    }

    // DELETE: api/admin/organization/{id}
    /// <summary>
    ///     Delete organization by id.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await organizationRepository.DeleteAsync(id);

        return NoContent();
    }
}
