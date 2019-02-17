using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Models.Identity;
using FunderMaps.Identity;
using FunderMaps.Extensions;

namespace FunderMaps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
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
            public string GivenName { get; set; }
            public string LastName { get; set; }
            public string Avatar { get; set; }
            public string JobTitle { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }

        // GET: api/authentication
        [Authorize]
        [HttpGet]
        public async Task<UserOutputModel> Get()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            return new UserOutputModel
            {
                GivenName = user.GivenName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                JobTitle = user.JobTitle,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public sealed class LoginInputModel
        {
            [Required]
            [EmailAddress]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool IsPersistent { get; set; } = false;
        }

        // POST: api/authentication/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> SignInAsync([FromBody] LoginInputModel input)
        {
            // Signout any previous sessions
            await _signInManager.SignOutAsync();

            // Authenticate and return the token as cookie
            var result = await _signInManager.PasswordSignInAsync(input.Email,
                input.Password,
                isPersistent: input.IsPersistent,
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
                return Ok(token.WriteToken());
            }
            return NotFound();
        }

        // POST: api/authentication/destroy
        [Authorize]
        [HttpPost("destroy")]
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
        [Authorize]
        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordInputModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest();
            }

            await _signInManager.RefreshSignInAsync(user);
            return Ok();
        }

        // POST: api/authentication/verification_email
        [Authorize]
        [HttpPost("verification_email")]
        public async Task<IActionResult> SendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // TODO: Send email with registration link

            return Ok();
        }
    }
}
