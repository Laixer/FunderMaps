using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for organization operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Authorize, Route("api/organization")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationRepository _organizationRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OrganizationController(IOrganizationRepository organizationRepository)
    {
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
    }

    // GET: organization
    /// <summary>
    ///     Return session organization.
    /// </summary>
    [HttpGet]
    public async Task<Organization> GetAsync()
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        return await _organizationRepository.GetByIdAsync(tenantId);
    }

    // PUT: organization
    /// <summary>
    ///     Update session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] Organization organization)
    {
        organization.Id = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await _organizationRepository.UpdateAsync(organization);

        return NoContent();
    }
}
