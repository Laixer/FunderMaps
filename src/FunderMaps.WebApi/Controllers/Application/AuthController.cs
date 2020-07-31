using FunderMaps.Controllers;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Interfaces;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for application authentication.
    /// </summary>
    [ApiController, Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly AuthManager _authManager;
        private readonly ISecurityTokenProvider _tokenProvider;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public AuthController(AuthManager authManager, ISecurityTokenProvider tokenProvider)
        {
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        [HttpGet("signin")]
        public async Task<IActionResult> SignInViaGetAsync([FromQuery] SignInDto input)
        {
            var result = await _authManager.PasswordSignInAsync(input.Email, input.Password, JwtBearerDefaults.AuthenticationScheme).ConfigureAwait(false);
            switch (result.Result)
            {
                case AuthResult.Success:
                    {
                        if (result.Principal == null)
                        {
                            throw new InvalidOperationException(); // TODO:
                        }
                        var token = await _tokenProvider.GetTokenAsStringAsync(result.Principal).ConfigureAwait(false);
                        return Ok(new { Token = token });
                    }
                case AuthResult.Failed:
                case AuthResult.LockedOut:
                case AuthResult.NotAllowed:
                    return Unauthorized();
            }

            // If we got this far, something failed
            throw new InvalidOperationException();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInViaPostAsync([FromBody] SignInDto input)
        {
            var result = await _authManager.PasswordSignInAsync(input.Email, input.Password, JwtBearerDefaults.AuthenticationScheme).ConfigureAwait(false);
            switch (result.Result)
            {
                case AuthResult.Success:
                    {
                        if (result.Principal == null)
                        {
                            throw new InvalidOperationException(); // TODO:
                        }
                        var token = await _tokenProvider.GetTokenAsStringAsync(result.Principal).ConfigureAwait(false);
                        return Ok(new { Token = token });
                    }
                case AuthResult.Failed:
                case AuthResult.LockedOut:
                case AuthResult.NotAllowed:
                    return Unauthorized();
            }

            // If we got this far, something failed
            throw new InvalidOperationException();
        }

        [HttpGet("token-refresh")]
        public async Task<IActionResult> RefreshSignInAsync()
        {
            var result = await _authManager.SignInAsync(User, checkIfAuthenticated: true, JwtBearerDefaults.AuthenticationScheme).ConfigureAwait(false);
            switch (result.Result)
            {
                case AuthResult.Success:
                    {
                        if (result.Principal == null)
                        {
                            throw new InvalidOperationException(); // TODO:
                        }
                        var token = await _tokenProvider.GetTokenAsStringAsync(result.Principal).ConfigureAwait(false);
                        return Ok(new { Token = token });
                    }
                case AuthResult.Failed:
                case AuthResult.LockedOut:
                case AuthResult.NotAllowed:
                    return Unauthorized();
            }

            // If we got this far, something failed
            throw new InvalidOperationException();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
