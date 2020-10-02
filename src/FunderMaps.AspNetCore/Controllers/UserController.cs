using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.AspNetCore.Controllers
{
    /// <summary>
    ///     Endpoint controller for user operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AuthManager _authManager;
        private readonly UserManager _userManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UserController(IMapper mapper, AuthManager authManager, UserManager userManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        // GET: api/user
        /// <summary>
        ///     Return session user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Act.
            User sessionUser = await _authManager.GetUserAsync(User);

            // Map.
            var output = _mapper.Map<UserDto>(sessionUser);

            // Return.
            return Ok(output);
        }

        // PUT: api/user
        /// <summary>
        ///     Update session user user.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            User sessionUser = await _authManager.GetUserAsync(User);
            user.Id = sessionUser.Id;

            // Act.
            await _userManager.UpdateAsync(user).ConfigureAwait(false);

            // Return.
            return NoContent();
        }

        // PUT: api/change-password
        /// <summary>
        ///     Update password for session user.
        /// </summary>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto input)
        {
            // Act.
            User sessionUser = await _authManager.GetUserAsync(User);

            // Act.
            await _userManager.ChangePasswordAsync(sessionUser, input.OldPassword, input.NewPassword).ConfigureAwait(false);

            // Return.
            return NoContent();
        }

    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
