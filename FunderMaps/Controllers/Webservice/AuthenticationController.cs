using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Models.Identity;
using FunderMaps.Identity;
using FunderMaps.Extensions;
using FunderMaps.Helpers;
using FunderMaps.Models;

namespace FunderMaps.Controllers.Webservice
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : AbstractMicroController
    {
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly SignInManager<FunderMapsUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<FunderMapsUser> userManager,
            SignInManager<FunderMapsUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public sealed class UserOutputModel
        {
            public Guid Id { get; set; }
            public bool? TwoFactorEnabled { get; set; }
            public bool? EmailConfirmed { get; set; }
            public bool? LockoutEnabled { get; set; }
            public bool? PhoneNumberConfirmed { get; set; }
            public int AccessFailedCount { get; set; }
            public DateTimeOffset? LockoutEnd { get; set; }
            public string Email { get; set; }
            public int? AttestationPrincipalId { get; set; }
            public IList<string> Roles { get; set; }
            public IList<Claim> Claims { get; set; }
        }

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
        public async Task<IActionResult> GetAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
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
                AttestationPrincipalId = user.AttestationPrincipalId,
                Roles = await _userManager.GetRolesAsync(user),
                Claims = await _userManager.GetClaimsAsync(user),
            });
        }

        public sealed class AuthenticationInputModel
        {
            [Required]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public sealed class AuthenticationOutputModel
        {
            public UserOutputModel User { get; set; }

            public string Token { get; set; }
        }

        // POST: api/authentication/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> SignInAsync([FromBody] AuthenticationInputModel input)
        {
            // Signout any previous sessions
            await _signInManager.SignOutAsync();

            // Authenticate and return the token as cookie
            var result = await _signInManager.PasswordSignInAsync(input.Email,
                input.Password,
                isPersistent: false,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // FUTURE: Retrieve signed in user via singing manager.
                var user = await _userManager.Users.SingleOrDefaultAsync(s => s.Email == input.Email);
                var token = new JwtTokenIdentityUser<FunderMapsUser, Guid>(user, _configuration.GetJwtSignKey())
                {
                    Issuer = _configuration.GetJwtIssuer(),
                    Audience = _configuration.GetJwtAudience(),
                    TokenValid = _configuration.GetJwtTokenExpirationInMinutes(),
                };
                token.AddRoleClaims(await _userManager.GetRolesAsync(user));

                return Ok(new AuthenticationOutputModel
                {
                    User = new UserOutputModel
                    {
                        Id = user.Id,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        EmailConfirmed = user.EmailConfirmed,
                        LockoutEnabled = user.LockoutEnabled,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        AccessFailedCount = user.AccessFailedCount,
                        LockoutEnd = user.LockoutEnd,
                        Email = user.Email,
                        AttestationPrincipalId = user.AttestationPrincipalId,
                        Roles = await _userManager.GetRolesAsync(user),
                        Claims = await _userManager.GetClaimsAsync(user),
                    },
                    Token = token.WriteToken()
                });
            }
            else if (result.IsLockedOut)
            {
                return Unauthorized(101, "Principal is locked out, contact the administrator");
            }
            else if (result.IsNotAllowed)
            {
                return Unauthorized(102, "Principal is not allowed to login");
            }
            else if (result.RequiresTwoFactor)
            {
                // TODO:
            }

            return Unauthorized(103, "Invalid credentials provided");
        }

        // POST: api/authentication/signout
        /// <summary>
        /// Signout all current sessions. With token based
        /// authentication this is not strictly required.
        /// </summary>
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        public sealed class ChangePasswordInputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string NewPassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string OldPassword { get; set; }
        }

        // POST: api/authentication/change_password
        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordInputModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ResourceNotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(IdentityErrorResponse(changePasswordResult.Errors));
            }

            await _signInManager.RefreshSignInAsync(user);
            return Ok();
        }

        public sealed class SetPasswordInputModel
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        // POST: api/authentication/set_password
        [Authorize(Roles = Constants.AdministratorRole)]
        [HttpPost("set_password")]
        public async Task<IActionResult> SetPasswordAsync([FromBody] SetPasswordInputModel input)
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

            await _signInManager.RefreshSignInAsync(user);
            return Ok();
        }

        // FUTURE: Fix
        // POST: api/authentication/confirm_email
        [AllowAnonymous]
        [HttpPost("confirm_email")]
        public async Task<IActionResult> SendConfirmEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return ResourceNotFound();
            }

            // Skip if email is confirmed already.
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return Ok();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // TODO: Send email with registration link

            return Ok();
        }
    }
}
