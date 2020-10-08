using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Exceptions;
using FunderMaps.Core.Interfaces.Repositories;
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
        private readonly Core.AppContext _appContext;
        private readonly IUserRepository _userRepository;
        private readonly SignInService _signinService;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public UserController(IMapper mapper, Core.AppContext appContext, IUserRepository userRepository, SignInService signinService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _signinService = signinService ?? throw new ArgumentNullException(nameof(signinService));
        }

        // GET: api/user
        /// <summary>
        ///     Return session user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Act.
            User sessionUser = await _userRepository.GetByIdAsync(_appContext.UserId);

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
            user.Id = _appContext.UserId;

            // Act.
            await _userRepository.UpdateAsync(user);

            // Return.
            return NoContent();
        }

        // POST: api/user/change-password
        /// <summary>
        ///     Set password for session user.
        /// </summary>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto input)
        {
            // Act.
            if (!await _signinService.CheckPasswordAsync(_appContext.UserId, input.OldPassword))
            {
                throw new InvalidCredentialException();
            }

            await _signinService.SetPasswordAsync(_appContext.UserId, input.NewPassword);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
