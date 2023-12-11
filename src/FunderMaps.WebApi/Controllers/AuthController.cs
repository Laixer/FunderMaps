using System.Security.Authentication;
using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for application authentication.
/// </summary>
[Authorize, Route("api/auth")]
public class AuthController(SignInService signInService, ISecurityTokenProvider tokenProvider) : ControllerBase
{
    // TODO: Get this from base controller.
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

    // POST: api/auth/signin
    /// <summary>
    ///     User sign in endpoint.
    /// </summary>
    /// <response code="200">Returns the security token on success.</response>
    /// <response code="401">When authentication failed.</response>
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<SignInSecurityTokenDto> SignInAsync([FromBody] SignInDto input)
    {
        var principal = await signInService.PasswordSignInAsync(input.Email, input.Password, "FunderMapsHybridAuth");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        var tokenContext = tokenProvider.GetTokenContext(principal);
        return new SignInSecurityTokenDto()
        {
            Id = tokenContext.Token.Id,
            Issuer = tokenContext.Token.Issuer,
            Token = tokenContext.TokenString,
            ValidFrom = tokenContext.Token.ValidFrom,
            ValidTo = tokenContext.Token.ValidTo,
        };
    }

    // GET: api/auth/token-refresh
    /// <summary>
    ///     Refresh access token for user.
    /// </summary>
    /// <response code="200">Returns the security token on success.</response>
    /// <response code="401">When authentication failed.</response>
    [HttpGet("token-refresh")]
    public async Task<SignInSecurityTokenDto> RefreshSignInAsync()
    {
        var principal = await signInService.UserIdSignInAsync(UserId, "FunderMapsHybridAuth");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        var tokenContext = tokenProvider.GetTokenContext(principal);
        return new SignInSecurityTokenDto()
        {
            Id = tokenContext.Token.Id,
            Issuer = tokenContext.Token.Issuer,
            Token = tokenContext.TokenString,
            ValidFrom = tokenContext.Token.ValidFrom,
            ValidTo = tokenContext.Token.ValidTo,
        };
    }

    // POST: api/auth/change-password
    /// <summary>
    ///     Set password for session user.
    /// </summary>
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto input)
    {
        if (!await signInService.CheckPasswordAsync(UserId, input.OldPassword))
        {
            throw new InvalidCredentialException();
        }

        await signInService.SetPasswordAsync(UserId, input.NewPassword);

        return NoContent();
    }
}