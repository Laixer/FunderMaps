using System.Security.Claims;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for user operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Authorize, Route("api/user")]
public class UserController(IUserRepository userRepository, SignInService signInService) : ControllerBase
{
    // GET: user
    /// <summary>
    ///     Return session user.
    /// </summary>
    [HttpGet]
    public async Task<User> GetAsync()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

        return await userRepository.GetByIdAsync(userId);
    }

    // PUT: user
    /// <summary>
    ///     Update session user user.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] User user)
    {
        user.Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

        await userRepository.UpdateAsync(user);

        return NoContent();
    }

    // POST: user/change-password
    /// <summary>
    ///     Set password for session user.
    /// </summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto input)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

        if (!await signInService.CheckPasswordAsync(userId, input.OldPassword))
        {
            throw new InvalidCredentialException();
        }

        await signInService.SetPasswordAsync(userId, input.NewPassword);

        return NoContent();
    }
}
