using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
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
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly SignInHandler _authenticationHelper;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthController(SignInHandler authenticationHelper)
            => _authenticationHelper = authenticationHelper ?? throw new ArgumentNullException(nameof(authenticationHelper));

        // POST: api/auth/signin
        /// <summary>
        ///     User sign in endpoint.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInInputModel input)
        {
            // Act.
            string token = await _authenticationHelper.SignInAsync(input.Email, input.Password);

            // Map.
            var output = new SignInSecurityTokenDto
            {
                Token = token,
                TokenValidity = 2400, // TODO
            };

            // Return.
            return Ok(output);
        }

        // GET: api/auth/token-refresh
        /// <summary>
        ///     Refresh access token for user.
        /// </summary>
        [HttpGet("token-refresh")]
        public async Task<IActionResult> RefreshSignInAsync()
        {
            // Act.
            string token = await _authenticationHelper.RefreshSignInAsync(User);

            // Map.
            var output = new SignInSecurityTokenDto
            {
                Token = token,
                TokenValidity = 2400, // TODO
            };

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
