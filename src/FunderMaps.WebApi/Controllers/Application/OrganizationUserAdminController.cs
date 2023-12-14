using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers.Application;

/// <summary>
///     Endpoint controller for organization user administration.
/// </summary>
/// <remarks>
///     This controller provides organization administration.
///     <para>
///         For the variant based on the current session see 
///         <see cref="AspNetCore.Controllers.OrganizationUserController"/>.
///     </para>
/// </remarks>
[Authorize(Policy = "AdministratorPolicy")]
[Route("api/admin/organization/{id:guid}/user")]
public sealed class OrganizationUserAdminController(
    IUserRepository userRepository,
    IOrganizationUserRepository organizationUserRepository,
    SignInService signInService) : FunderMapsController
{
    // POST: api/admin/organization/{id}/user
    /// <summary>
    ///     Add user to organization.
    /// </summary>
    // [HttpPost]
    // public async Task<User> AddUserAsync(Guid id, [FromBody] user + role + passwd input)
    // {
    //     var user = _mapper.Map<User>(input);

    //     // FUTURE: Do in 1 call.
    //     user = await _userRepository.AddGetAsync(user);
    //     await _signInService.SetPasswordAsync(user.Id, input.Password);
    //     await _organizationUserRepository.AddAsync(id, user.Id, input.OrganizationRole);

    //     return user;
    // }

    // GET: api/admin/organization/{id}/user
    /// <summary>
    ///     Get all users in the organization.
    /// </summary>
    [HttpGet]
    public IAsyncEnumerable<OrganizationUser> GetAllUserAsync(Guid id, [FromQuery] PaginationDto pagination)
        => organizationUserRepository.ListAllAsync(id, pagination.Navigation);

    // PUT: api/admin/organization/{id}/user/{id}
    /// <summary>
    ///     Update user in the organization.
    /// </summary>
    [HttpPut("{userId:guid}")]
    public async Task<IActionResult> UpdateUserAsync(Guid id, Guid userId, [FromBody] User user)
    {
        user.Id = userId;

        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(id, user.Id))
        {
            throw new AuthorizationException();
        }
        await userRepository.UpdateAsync(user);

        return NoContent();
    }

    // POST: api/organization/user/{id}/change-organization-role
    /// <summary>
    ///     Set user organization role in the session organization.
    /// </summary>
    [HttpPost("{userId:guid}/change-organization-role")]
    public async Task<IActionResult> ChangeOrganizationUserRoleAsync(Guid id, Guid userId, [FromBody] ChangeOrganizationRoleDto input)
    {
        // Act.
        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(id, userId))
        {
            throw new AuthorizationException();
        }
        await organizationUserRepository.SetOrganizationRoleByUserIdAsync(userId, input.Role);

        // Return.
        return NoContent();
    }

    // FUTURE: Remove old password from DTO
    // POST: api/organization/user/{id}/change-password
    /// <summary>
    ///     Set user password in the session organization.
    /// </summary>
    [HttpPost("{userId:guid}/change-password")]
    public async Task<IActionResult> ChangePasswordAsync(Guid id, Guid userId, [FromBody] ChangePasswordDto input)
    {
        // Act.
        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(id, userId))
        {
            throw new AuthorizationException();
        }

        // Act.
        await signInService.SetPasswordAsync(userId, input.NewPassword);

        // Return.
        return NoContent();
    }

    // DELETE: api/admin/organization/{id}/user/{id}
    /// <summary>
    ///     Delete user in the organization.
    /// </summary>
    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id, Guid userId)
    {
        // Act.
        // TODO: Move to db
        if (!await organizationUserRepository.IsUserInOrganization(id, userId))
        {
            throw new AuthorizationException();
        }
        await userRepository.DeleteAsync(userId);

        // Return.
        return NoContent();
    }
}
