using AutoMapper;
using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.AspNetCore.Controllers
{
    /// <summary>
    ///     Endpoint controller for application authentication.
    /// </summary>
    [Authorize, Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly SignInHandler _authenticationHelper;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthController(IMapper mapper, SignInHandler authenticationHelper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authenticationHelper = authenticationHelper ?? throw new ArgumentNullException(nameof(authenticationHelper));
        }

        // POST: auth/signin
        /// <summary>
        ///     User sign in endpoint.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInDto input)
        {
            // Act.
            TokenContext context = await _authenticationHelper.SignInAsync(input.Email, input.Password);

            // Map.
            var output = _mapper.Map<SignInSecurityTokenDto>(context);

            // Return.
            return Ok(output);
        }

        // GET: auth/token-refresh
        /// <summary>
        ///     Refresh access token for user.
        /// </summary>
        [HttpGet("token-refresh")]
        public async Task<IActionResult> RefreshSignInAsync()
        {
            // Act.
            TokenContext context = await _authenticationHelper.RefreshSignInAsync(User);

            // Map.
            var output = _mapper.Map<SignInSecurityTokenDto>(context);

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
