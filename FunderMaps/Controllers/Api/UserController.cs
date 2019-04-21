using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Models.Identity;
using FunderMaps.Models;

namespace FunderMaps.Controllers.Api
{
    /// <summary>
    /// Current user profile operations.
    /// </summary>
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly UserManager<FunderMapsUser> _userManager;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="userManager">See <see cref="UserManager{TUser}"/>.</param>
        public UserController(UserManager<FunderMapsUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// User profile model.
        /// </summary>
        public sealed class UserInputOutputModel
        {
            /// <summary>
            /// Gets or sets the given name for the user.
            /// </summary>
            public string GivenName { get; set; }

            /// <summary>
            /// Gets or sets the last name for the user.
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// Gets or sets a user avatar.
            /// </summary>
            public string Avatar { get; set; }

            /// <summary>
            /// Gets or sets the job title for the user.
            /// </summary>
            public string JobTitle { get; set; }

            /// <summary>
            /// Gets or sets the phone number for the user.
            /// </summary>
            public string PhoneNumber { get; set; }
        }

        // GET: api/user
        /// <summary>
        /// Get the profile of the current authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserInputOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            return Ok(new UserInputOutputModel
            {
                GivenName = user.GivenName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                JobTitle = user.JobTitle,
                PhoneNumber = user.PhoneNumber,
            });
        }

        // PUT: api/user
        /// <summary>
        /// Update profile of the current authenticated user.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> PutAsync([FromBody] UserInputOutputModel input)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            user.GivenName = input.GivenName;
            user.LastName = input.LastName;
            user.Avatar = input.Avatar;
            user.JobTitle = input.JobTitle;
            user.PhoneNumber = input.PhoneNumber;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
