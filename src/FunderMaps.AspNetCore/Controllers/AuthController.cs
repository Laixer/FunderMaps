using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for application authentication.
/// </summary>
[Authorize, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInService _signInService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public AuthController(SignInService signInService)
        => _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));

    // POST: auth/signin
    /// <summary>
    ///     User sign in endpoint.
    /// </summary>
    /// <response code="200">Returns the security token on success.</response>
    /// <response code="401">When authentication failed.</response>
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<SignInSecurityTokenDto> SignInAsync([FromBody] SignInDto input)
    {
        var context = await _signInService.PasswordSignInAsync(input.Email, input.Password);

        return new SignInSecurityTokenDto()
        {
            Id = context.Token.Id,
            Issuer = context.Token.Issuer,
            Token = context.TokenString,
            ValidFrom = context.Token.ValidFrom,
            ValidTo = context.Token.ValidTo,
        };
    }

    // GET: auth/token-refresh
    /// <summary>
    ///     Refresh access token for user.
    /// </summary>
    /// <response code="200">Returns the security token on success.</response>
    /// <response code="401">When authentication failed.</response>
    [HttpGet("token-refresh")]
    public async Task<SignInSecurityTokenDto> RefreshSignInAsync()
    {
        var context = await _signInService.SignInAsync(User);

        return new SignInSecurityTokenDto()
        {
            Id = context.Token.Id,
            Issuer = context.Token.Issuer,
            Token = context.TokenString,
            ValidFrom = context.Token.ValidFrom,
            ValidTo = context.Token.ValidTo,
        };
    }
}
