using System.Security.Claims;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for application authentication.
/// </summary>
/// <remarks>
///     Create new instance.
/// </remarks>
[Authorize, Route("api/auth")]
public class AuthController(SignInService signInService, ISecurityTokenProvider tokenProvider) : ControllerBase
{
    private readonly SignInService _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    private readonly ISecurityTokenProvider _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

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
        var principal = await _signInService.PasswordSignInAsync(input.Email, input.Password, "FunderMapsHybridAuth");

        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        var tokenContext = _tokenProvider.GetTokenContext(principal);

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
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

        var principal = await _signInService.UserIdSignInAsync(userId, "FunderMapsHybridAuth");

        var authProperties = new AuthenticationProperties();

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        var tokenContext = _tokenProvider.GetTokenContext(principal);

        return new SignInSecurityTokenDto()
        {
            Id = tokenContext.Token.Id,
            Issuer = tokenContext.Token.Issuer,
            Token = tokenContext.TokenString,
            ValidFrom = tokenContext.Token.ValidFrom,
            ValidTo = tokenContext.Token.ValidTo,
        };
    }
}
