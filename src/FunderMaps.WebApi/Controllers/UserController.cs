using FunderMaps.Core.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for user operations.
/// </summary>
/// <remarks>
///     This controller should *only* handle operations on the current
///     user session. Therefore the user context must be active.
/// </remarks>
[Route("api/user")]
public class UserController(IUserRepository userRepository) : FunderMapsController
{
    // GET: user
    /// <summary>
    ///     Return session user.
    /// </summary>
    [HttpGet]
    public Task<User> GetAsync()
        => userRepository.GetByIdAsync(UserId);

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
