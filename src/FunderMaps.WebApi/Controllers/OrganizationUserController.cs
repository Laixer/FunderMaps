using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for organization user operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle organization operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Route("api/organization/user")]
public class OrganizationUserController(IOrganizationUserRepository organizationUserRepository) : FunderMapsController
{
    // GET: organization/user
    /// <summary>
    ///     Get all users in the session organization.
    /// </summary>
    [HttpGet]
    public async Task<IEnumerable<OrganizationUser>> GetAllUserAsync([FromQuery] PaginationDto pagination)
        => await organizationUserRepository.ListAllAsync(TenantId, pagination.Navigation).ToListAsync();
}
