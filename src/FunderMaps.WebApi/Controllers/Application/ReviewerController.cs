using AutoMapper;
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
public class ReviewerController : ControllerBase
{
    private readonly Core.AppContext _appContext;
    private readonly IOrganizationUserRepository _organizationUserRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public ReviewerController(IMapper mapper, Core.AppContext appContext, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    // GET: api/reviewer
    /// <summary>
    ///     Return all reviewers.
    /// </summary>
    [HttpGet("reviewer"), ResponseCache(Duration = 60 * 60, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
    public async IAsyncEnumerable<User> GetAllAsync([FromQuery] PaginationDto pagination)
    {
        var roles = new OrganizationRole[] { OrganizationRole.Verifier, OrganizationRole.Superuser };
        await foreach (var user in _organizationUserRepository.ListAllByRoleAsync(_appContext.TenantId, roles, pagination.Navigation))
        {
            yield return await _userRepository.GetByIdAsync(user);
        }
    }
}
