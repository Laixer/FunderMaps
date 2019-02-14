using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Models.Identity;

namespace FunderMaps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly SignInManager<FunderMapsUser> _signInManager;

        public AuthenticationController(UserManager<FunderMapsUser> userManager, SignInManager<FunderMapsUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                return Ok();
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
    }
}
