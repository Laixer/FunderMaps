using System.Security.Authentication;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Controllers;
using FunderMaps.Core.DataTransferObjects;
using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.WebApi.Controllers;

/// <summary>
///     Endpoint controller for application authentication.
/// </summary>
[Route("api/auth")]
public class AuthController(SignInService signInService, ISecurityTokenProvider tokenProvider) : FunderMapsController
{
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

    // POST: api/auth/reset-password
    /// <summary>
    ///     Send password reset email.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto input)
    {
        await signInService.ResetPasswordAsync(input.Email);

        return NoContent();
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
