using FunderMaps.Controllers;
using FunderMaps.WebApi.Authentication;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for application authentication.
    /// </summary>
    [Authorize]
    [ApiController, Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly AuthenticationHelper _authenticationHelper;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthController(AuthenticationHelper authenticationHelper)
        {
            _authenticationHelper = authenticationHelper ?? throw new ArgumentNullException(nameof(authenticationHelper));
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInDto input)
        {
            // Act.
            string token = await _authenticationHelper.SignInAsync(input.Email, input.Password);

            // Map.
            var output = new SignInSecurityTokenDto
            {
                Token = token,
                TokenValidity = 2400,
            };

            // Return.
            return Ok(output);
        }

        [HttpGet("token-refresh")]
        public async Task<IActionResult> RefreshSignInAsync()
        {
            // Act.
            string token = await _authenticationHelper.RefreshSignInAsync(User);

            // Map.
            var output = new SignInSecurityTokenDto
            {
                Token = token,
                TokenValidity = 2400,
            };

            // Return.
            return Ok(output);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
