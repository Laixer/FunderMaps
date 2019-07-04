using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Models.Identity;
using FunderMaps.ViewModels;
using FunderMaps.Data;
using FunderMaps.Event;
using FunderMaps.Middleware;
using FunderMaps.Core.Event;

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
        private readonly IEventService _eventService;

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="userManager">See <see cref="UserManager{TUser}"/>.</param>
        /// <param name="fisContext">See <see cref="FisDbContext"/>.</param>
        public UserController(UserManager<FunderMapsUser> userManager, FisDbContext fisContext, IEventService eventService, System.IServiceProvider sp)
        {
            _userManager = userManager;
            _eventService = eventService;
        }

        // GET: api/user
        /// <summary>
        /// Get the profile of the current authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ProfileInputOutputModel), 200)]
        [ProducesResponseType(typeof(ErrorOutputModel), 404)]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return ResourceNotFound();
            }

            return Ok(new ProfileInputOutputModel
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
        public async Task<IActionResult> PutAsync([FromBody] ProfileInputOutputModel input)
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

            await _eventService.FireEventAsync(new UpdateUserProfileEvent
            {
                User = user
            });

            return NoContent();
        }
    }
}
