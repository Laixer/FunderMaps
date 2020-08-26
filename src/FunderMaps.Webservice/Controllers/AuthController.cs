using FunderMaps.AspNetCore.Authentication;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.AspNetCore.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.Webservice.Controllers
{
    // NOTE: This is copied from the WebApi.
    /// <summary>
    ///     Endpoint controller for application authentication.
    /// </summary>
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationHelper _authenticationHelper;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthController(AuthenticationHelper authenticationHelper)
            => _authenticationHelper = authenticationHelper ?? throw new ArgumentNullException(nameof(authenticationHelper));

        /// <summary>
        ///     User sign in endpoint.
        /// </summary>
        /// <param name="input"><see cref="SignInInputModel"/></param>
        /// <returns><see cref="OkObjectResult"/></returns>
        [AllowAnonymous]
        [HttpPost("signin")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SignInSecurityTokenDto))]
        public async Task<IActionResult> SignInAsync([FromBody] SignInInputModel input)
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

        /// <summary>
        ///     Refresh access token for user.
        /// </summary>
        /// <returns><see cref="OkObjectResult"/></returns>
        [HttpGet("token-refresh")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SignInSecurityTokenDto))]
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
