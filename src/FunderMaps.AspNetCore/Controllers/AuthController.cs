using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FunderMaps.AspNetCore.Controllers;

/// <summary>
///     Endpoint controller for application authentication.
/// </summary>
[Authorize, Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly SignInService _signInService;

    /// <summary>
    ///     Create new instance.
    /// </summary>
    public AuthController(IMapper mapper, SignInService signInService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    }

    // POST: auth/signin
    /// <summary>
    ///     User sign in endpoint.
    /// </summary>
    /// <response code="200">Returns the security token on success.</response>
    /// <response code="401">When authentication failed.</response>
    [AllowAnonymous]
    [HttpPost("signin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SignInSecurityTokenDto>> SignInAsync([FromBody] SignInDto input)
    {
        TokenContext context = await _signInService.PasswordSignInAsync(input.Email, input.Password);

        var output = _mapper.Map<SignInSecurityTokenDto>(context);

        return Ok(output);
    }

    // GET: auth/token-refresh
    /// <summary>
    ///     Refresh access token for user.
    /// </summary>
    /// <response code="200">Returns the security token on success.</response>
    /// <response code="401">When authentication failed.</response>
    [HttpGet("token-refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SignInSecurityTokenDto>> RefreshSignInAsync()
    {
        TokenContext context = await _signInService.SignInAsync(User);

        var output = _mapper.Map<SignInSecurityTokenDto>(context);

        return Ok(output);
    }
}
