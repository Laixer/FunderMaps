using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for organization user operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle organization operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Authorize, Route("api/organization/user")]
public class OrganizationUserController(
    IUserRepository userRepository,
    IOrganizationUserRepository organizationUserRepository,
    SignInService signInService) : FunderMapsController
{
    // GET: organization/user
    /// <summary>
    ///     Get all users in the session organization.
    /// </summary>
    [HttpGet]
    public async IAsyncEnumerable<OrganizationUser> GetAllUserAsync([FromQuery] PaginationDto pagination)
    {
        await foreach (var user in organizationUserRepository.ListAllAsync(TenantId, pagination.Navigation))
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
        user.Id = id;

        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(TenantId, user.Id))
        {
            throw new AuthorizationException();
        }
        await userRepository.UpdateAsync(user);

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
        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(TenantId, id))
        {
            throw new AuthorizationException();
        }
        await organizationUserRepository.SetOrganizationRoleByUserIdAsync(id, input.Role);

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
        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(TenantId, id))
        {
            throw new AuthorizationException();
        }

        if (input.NewPassword is not null)
        {
            await signInService.SetPasswordAsync(id, input.NewPassword);
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
        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(TenantId, id))
        {
            throw new AuthorizationException();
        }
        await userRepository.DeleteAsync(id);

        return NoContent();
    }
}
