using FunderMaps.Authorization;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Identity;
using FunderMaps.Interfaces;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.Controllers.Api
{
    // FUTURE: Replace IOrganizationRepository by OrganizationManager
    /// <summary>
    /// Authentication endpoint.
    /// </summary>
    [Authorize]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : BaseApiController
    {
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly SignInManager<FunderMapsUser> _signInManager;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public AuthenticationController(
            UserManager<FunderMapsUser> userManager,
            SignInManager<FunderMapsUser> signInManager,
            IOrganizationUserRepository organizationUserRepository,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _organizationUserRepository = organizationUserRepository;
            _configuration = configuration;
            _logger = logger;
        }

        // TODO: Extension?
        /// <summary>
        /// Map identity errors to error output model.
        /// </summary>
        /// <param name="errors">List of errors.</param>
        /// <returns>ErrorOutputModel.</returns>
        protected ErrorOutputModel IdentityErrorResponse(IEnumerable<IdentityError> errors)
        {
            var errorModel = new ErrorOutputModel();
            foreach (var item in errors)
            {
                errorModel.AddError(0, item.Description);
            }

            return errorModel;
        }

        // GET: api/authentication
        /// <summary>
        /// Get authentication principal properties related to
        /// the current authenticated object.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserOutputModel), 200)]
        public async Task<IActionResult> GetAsync()
        {
            // TODO: Should use `_userManager.GetUserAsync(User)` but
            // the NameIdentifier in User is set wrongly to email.

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            return Ok(new UserOutputModel
            {
                Id = user.Id,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,
                LockoutEnd = user.LockoutEnd,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            });
        }

        /// <summary>
        /// Generate authentication token for user.
        /// </summary>
        /// <param name="user">See <see cref="FunderMapsUser"/>.</param>
        /// <returns>AuthenticationOutputModel.</returns>
        private async Task<AuthenticationOutputModel> GenerateSecurityToken(FunderMapsUser user)
        {
            var token = new JwtTokenIdentityUser<FunderMapsUser, Guid>(user, _configuration.GetJwtSignKey())
            {
                Issuer = _configuration.GetJwtIssuer(),
                Audience = _configuration.GetJwtAudience(),
                TokenValid = _configuration.GetJwtTokenExpirationInMinutes(),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            // Add application role as claim.
            token.AddRoleClaims(userRoles);

            // Add organization as claim and corresponding organization role.
            var organizationUser = await _organizationUserRepository.GetByUserIdAsync(user.Id);
            if (organizationUser != null)
            {
                token.AddClaim(ClaimTypes.OrganizationUser, organizationUser.OrganizationId);
                token.AddClaim(ClaimTypes.OrganizationUserRole, organizationUser.Role);
            }

            return new AuthenticationOutputModel
            {
                Principal = new PrincipalOutputModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = userRoles,
                    Claims = token.Claims,
                },
                Token = token.WriteToken(),
                TokenValid = token.TokenValid,
            };
        }

        // POST: api/authentication/signin
        /// <summary>
        /// Authenticate user object.
        /// </summary>
        /// <param name="input">See <see cref="UserInputModel"/>.</param>
        /// <returns>See <see cref="AuthenticationOutputModel"/>.</returns>
        [AllowAnonymous]
        [HttpPost("signin")]
        [ProducesResponseType(typeof(AuthenticationOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> SignInAsync([FromBody] UserInputModel input)
        {
            // NOTE: Find user for authentication, if the user object cannot be found then return an
            //       authentication faillure since the client is not allowed to guess the credentials.
            //       We look for the user first since the signinManager sets cookie authentication on 
            //       the default login operation.
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed, unknown object {user}", input.Email);
                return Unauthorized(103, "Invalid credentials provided");
            }

            // Check the password for the found user.
            var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, true);
            if (result.IsLockedOut)
            {
                return Unauthorized(101, "Principal is locked out, contact the administrator");
            }
            else if (result.IsNotAllowed)
            {
                return Unauthorized(102, "Principal is not allowed to login");
            }
            else if (result.Succeeded)
            {
                _logger.LogInformation("Authentication successful, returning security token");

                return Ok(await GenerateSecurityToken(user));
            }

            // FUTURE: RequiresTwoFactor

            return Unauthorized(103, "Invalid credentials provided");
        }

        // GET: api/authentication/refresh
        /// <summary>
        /// Refresh authentication token.
        /// </summary>
        [HttpGet("refresh")]
        [ProducesResponseType(typeof(AuthenticationOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 401)]
        public async Task<IActionResult> RefreshSignInAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            if (await _signInManager.CanSignInAsync(user))
            {
                _logger.LogInformation("Authentication refreshed, returning security token");

                return Ok(await GenerateSecurityToken(user));
            }

            return Unauthorized(102, "Principal is not allowed to login");
        }

        // POST: api/authentication/change_password
        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="input">Password input model.</param>
        [HttpPost("change_password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordInputModel input)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(IdentityErrorResponse(changePasswordResult.Errors));
            }

            return NoContent();
        }

        // POST: api/authentication/set_password
        /// <summary>
        /// Set the password for a user. Only an administrator can do this.
        /// </summary>
        /// <param name="input">User password input model.</param>
        [Authorize(Roles = Constants.AdministratorRole)]
        [HttpPost("set_password")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        [ProducesResponseType(typeof(ErrorOutputModel), 400)]
        public async Task<IActionResult> SetPasswordAsync([FromBody] UserInputModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                return ResourceNotFound();
            }

            if (await _userManager.HasPasswordAsync(user))
            {
                await _userManager.RemovePasswordAsync(user);
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, input.Password);
            if (!addPasswordResult.Succeeded)
            {
                return BadRequest(IdentityErrorResponse(addPasswordResult.Errors));
            }

            return NoContent();
        }
    }
}
