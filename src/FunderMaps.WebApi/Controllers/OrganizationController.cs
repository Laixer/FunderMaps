using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
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
    [ResponseCache(Duration = 60 * 60, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
    public Task<Organization> GetAsync()
        => organizationRepository.GetByIdAsync(TenantId);
}
