using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
public class UserController : ControllerBase
{
    private readonly Core.AppContext _appContext;
    private readonly IUserRepository _userRepository;
    private readonly SignInService _signInService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public UserController(Core.AppContext appContext, IUserRepository userRepository, SignInService signInService)
    {
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    }

    // GET: user
    /// <summary>
    ///     Return session user.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<User> GetAsync()
        => await _userRepository.GetByIdAsync(_appContext.UserId);

    // PUT: user
    /// <summary>
    ///     Update session user user.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAsync([FromBody] User user)
    {
        user.Id = _appContext.UserId;

        await _userRepository.UpdateAsync(user);

        return NoContent();
    }

    // POST: user/change-password
    /// <summary>
    ///     Set password for session user.
    /// </summary>
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto input)
    {
        if (input.OldPassword is not null && input.NewPassword is not null)
        {
            if (!await _signInService.CheckPasswordAsync(_appContext.UserId, input.OldPassword))
            {
                throw new InvalidCredentialException();
            }

            await _signInService.SetPasswordAsync(_appContext.UserId, input.NewPassword);
        }

        return NoContent();
    }
}
