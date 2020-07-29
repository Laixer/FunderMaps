using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for user operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle user operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [ApiController, Route("api/user")]
    public class UserController : BaseApiController
    {
        private static readonly Guid testUserId = Guid.Parse("407988e2-e80c-4edd-85ab-2469c8beb15d"); // TODO: Get from context

        private readonly IMapper _mapper;
        private readonly UserManager _userManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UserController(IMapper mapper, UserManager userManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Act.
            User user = await _userManager.GetAsync(testUserId).ConfigureAwait(false);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            user.Id = testUserId;

            // Act.
            await _userManager.UpdateAsync(user).ConfigureAwait(false);

            // Return.
            return NoContent();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto input)
        {
            // Act.
            User user = await _userManager.GetAsync(testUserId).ConfigureAwait(false);

            // Act.
            await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword).ConfigureAwait(false);

            // Return.
            return NoContent();
        }

    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
