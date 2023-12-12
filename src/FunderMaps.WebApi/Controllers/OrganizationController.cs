using FunderMaps.AspNetCore.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for organization operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Route("api/organization")]
public class OrganizationController(IOrganizationRepository organizationRepository) : FunderMapsController
{
    // GET: organization
    /// <summary>
    ///     Return session organization.
    /// </summary>
    [HttpGet]
    public Task<Organization> GetAsync()
        => organizationRepository.GetByIdAsync(TenantId);

    // PUT: organization
    /// <summary>
    ///     Update session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] Organization organization)
    {
        organization.Id = TenantId;

        await organizationRepository.UpdateAsync(organization);

        return NoContent();
    }
}
