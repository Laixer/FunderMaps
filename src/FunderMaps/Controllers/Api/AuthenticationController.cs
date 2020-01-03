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
using Microsoft.Extensions.Localization;
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
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : BaseApiController
    {
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly SignInManager<FunderMapsUser> _signInManager;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IStringLocalizer<ReportController> _localizer;

        /// <summary>
        /// Create new instance.
        /// </summary>
        public AuthenticationController(
            UserManager<FunderMapsUser> userManager,
            SignInManager<FunderMapsUser> signInManager,
            IOrganizationUserRepository organizationUserRepository,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            IStringLocalizer<ReportController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _organizationUserRepository = organizationUserRepository;
            _configuration = configuration;
            _logger = logger;
            _localizer = localizer;
        }

        // TODO: Extension? Replace with Problem()
        /// <summary>
        /// Map identity errors to error output model.
        /// </summary>
        /// <param name="errors">List of errors.</param>
        /// <returns>ErrorOutputModel.</returns>
        protected static ErrorOutputModel IdentityErrorResponse(IEnumerable<IdentityError> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }

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
        public async Task<IActionResult> GetAsync()
        {
            // TODO: Should use `_userManager.GetUserAsync(User)` but
            //       the NameIdentifier in User is set wrongly to email.

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
        /// <returns><see cref="AuthenticationOutputModel"/>.</returns>
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

            // Add organization and corresponding organization role as claim.
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
        /// Authenticate user object and return a authentication token.
        /// </summary>
        /// <param name="input">See <see cref="UserInputModel"/>.</param>
        /// <returns>See <see cref="AuthenticationOutputModel"/>.</returns>
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] UserInputModel input)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }

            // NOTE: Find user for authentication, if the user object cannot be found then return an
            //       authentication faillure since the client is not allowed to guess the credentials.
            //       We look for the user first since the signinManager sets cookie authentication on 
            //       the default login operation.
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed, unknown object {user}", input.Email);

                return Unauthorized(103, _localizer["Invalid credentials provided"]);
            }

            // Check the password for the found user.
            var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, true);
            if (result.IsLockedOut)
            {
                return Unauthorized(101, _localizer["Principal is locked out, contact the administrator"]);
            }
            else if (result.IsNotAllowed)
            {
                return Unauthorized(102, _localizer["Principal is not allowed to login"]);
            }
            else if (result.Succeeded)
            {
                _logger.LogInformation(_localizer["Authentication successful, returning security token"]);

                return Ok(await GenerateSecurityToken(user));
            }

            // FUTURE: RequiresTwoFactor or any multifactor auth.

            return Unauthorized(103, _localizer["Invalid credentials provided"]);
        }

        // GET: api/authentication/refresh
        /// <summary>
        /// Refresh authentication token.
        /// </summary>
        [HttpGet("refresh")]
        public async Task<IActionResult> RefreshSignInAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            if (await _signInManager.CanSignInAsync(user))
            {
                _logger.LogInformation(_localizer["Authentication refreshed, returning security token"]);

                return Ok(await GenerateSecurityToken(user));
            }

            return Unauthorized(102, _localizer["Principal is not allowed to login"]);
        }

        // POST: api/authentication/set_password
        /// <summary>
        /// Set the password for a user. Only an administrator can do this.
        /// </summary>
        /// <param name="input">User password input model.</param>
        [Authorize(Roles = Constants.AdministratorRole)]
        [HttpPost("set_password")]
        public async Task<IActionResult> SetPasswordAsync([FromBody] UserInputModel input)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                return ResourceNotFound();
            }

            // NOTE: Password must be removed before it can be set.
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
