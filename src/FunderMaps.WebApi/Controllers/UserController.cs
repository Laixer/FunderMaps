using System.Security.Claims;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for user operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Authorize, Route("api/user")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    // TODO: Get this from base controller.
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

    // GET: user
    /// <summary>
    ///     Return session user.
    /// </summary>
    [HttpGet]
    public async Task<User> GetAsync()
        => await userRepository.GetByIdAsync(UserId);

    // PUT: user
    /// <summary>
    ///     Update session user user.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] User user)
    {
        user.Id = UserId;

        await userRepository.UpdateAsync(user);

        return NoContent();
    }
}
