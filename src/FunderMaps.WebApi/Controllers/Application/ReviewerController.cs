using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
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
public sealed class ReviewerController(
    IOrganizationUserRepository organizationUserRepository,
    IUserRepository userRepository) : FunderMapsController
{
    // GET: api/reviewer
    /// <summary>
    ///     Return all reviewers.
    /// </summary>
    [HttpGet("reviewer"), ResponseCache(Duration = 60 * 60, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
    public async IAsyncEnumerable<User> GetAllAsync([FromQuery] PaginationDto pagination)
    {
        var roles = new OrganizationRole[] { OrganizationRole.Verifier, OrganizationRole.Superuser };
        await foreach (var user in organizationUserRepository.ListAllByRoleAsync(TenantId, roles, pagination.Navigation))
        {
            yield return await userRepository.GetByIdAsync(user);
        }
    }
}
