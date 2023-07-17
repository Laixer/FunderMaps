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
    private readonly Core.AppContext _appContext;
    private readonly IOrganizationRepository _organizationRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OrganizationController(Core.AppContext appContext, IOrganizationRepository organizationRepository)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _organizationRepository = organizationRepository ?? throw new ArgumentNullException(nameof(organizationRepository));
    }

    // GET: organization
    /// <summary>
    ///     Return session organization.
    /// </summary>
    [HttpGet]
    public Task<Organization> GetAsync()
        => _organizationRepository.GetByIdAsync(_appContext.TenantId);

    // PUT: organization
    /// <summary>
    ///     Update session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] Organization organization)
    {
        organization.Id = _appContext.TenantId;

        await _organizationRepository.UpdateAsync(organization);

        return NoContent();
    }
}
