using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for organization user operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle organization operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Authorize, Route("api/organization/user")]
public class OrganizationUserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationUserRepository _organizationUserRepository;
    private readonly SignInService _signInService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public OrganizationUserController(
        IUserRepository userRepository,
        IOrganizationUserRepository organizationUserRepository,
        SignInService signInService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    }

    // GET: organization/user
    /// <summary>
    ///     Get all users in the session organization.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<OrganizationUser> GetAllUserAsync([FromQuery] PaginationDto pagination)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        await foreach (var user in _organizationUserRepository.ListAllAsync(tenantId, pagination.Navigation))
        {
            yield return user;
        }
    }

    // PUT: organization/user/{id}
    /// <summary>
    ///     Update user in the session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] User user)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        user.Id = id;

        // TODO: Move to db
        if (!await _organizationUserRepository.IsUserInOrganization(tenantId, user.Id))
        {
            throw new AuthorizationException();
        }
        await _userRepository.UpdateAsync(user);

        return NoContent();
    }

    // POST: organization/user/{id}/change-organization-role
    /// <summary>
    ///     Set user organization role in the session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpPost("{id:guid}/change-organization-role")]
    public async Task<IActionResult> ChangeOrganizationUserRoleAsync(Guid id, [FromBody] ChangeOrganizationRoleDto input)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        // TODO: Move to db
        if (!await _organizationUserRepository.IsUserInOrganization(tenantId, id))
        {
            throw new AuthorizationException();
        }
        await _organizationUserRepository.SetOrganizationRoleByUserIdAsync(id, input.Role);

        return NoContent();
    }

    // FUTURE: Remove old password from DTO
    // POST: organization/user/{id}/change-password
    /// <summary>
    ///     Set user password in the session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpPost("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePasswordAsync(Guid id, [FromBody] ChangePasswordDto input)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        // TODO: Move to db
        if (!await _organizationUserRepository.IsUserInOrganization(tenantId, id))
        {
            throw new AuthorizationException();
        }

        if (input.NewPassword is not null)
        {
            await _signInService.SetPasswordAsync(id, input.NewPassword);
        }

        return NoContent();
    }

    // DELETE: organization/user/{id}
    /// <summary>
    ///     Delete user in the session organization.
    /// </summary>
    [Authorize(Policy = "SuperuserPolicy")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        var tenantId = Guid.Parse(User.FindFirstValue(FunderMapsAuthenticationClaimTypes.Tenant) ?? throw new InvalidOperationException());

        // TODO: Move to db
        if (!await _organizationUserRepository.IsUserInOrganization(tenantId, id))
        {
            throw new AuthorizationException();
        }
        await _userRepository.DeleteAsync(id);

        return NoContent();
    }
}
