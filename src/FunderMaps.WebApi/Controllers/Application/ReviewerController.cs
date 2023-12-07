using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for application reviewers.
/// </summary>
/// <remarks>
///     This controller should *only* handle operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Authorize(Policy = "WriterPolicy")]
[Route("api")]
public class ReviewerController(
    IOrganizationUserRepository organizationUserRepository,
    IUserRepository userRepository) : ControllerBase
{
    // GET: api/reviewer
    /// <summary>
    ///     Return all reviewers.
    /// </summary>
    [HttpGet("reviewer"), ResponseCache(Duration = 60 * 60, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
    public async IAsyncEnumerable<User> GetAllAsync([FromQuery] PaginationDto pagination)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        var roles = new OrganizationRole[] { OrganizationRole.Verifier, OrganizationRole.Superuser };
        await foreach (var user in organizationUserRepository.ListAllByRoleAsync(tenantId, roles, pagination.Navigation))
        {
            yield return await userRepository.GetByIdAsync(user);
        }
    }
}
