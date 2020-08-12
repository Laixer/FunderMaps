using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization user operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle organization operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [ApiController, Route("api/organization/user")]
    public class OrganizationUserController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly AuthManager _authManager;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserController(IMapper mapper, AuthManager authManager, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);
            user = await _organizationManager.AddUserAsync(sessionOrganization.Id, user);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] PaginationModel pagination)
        {
            // Act.
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);
            IAsyncEnumerable<User> userList = _organizationManager.GetAllUserAsync(sessionOrganization.Id, pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<UserDto>, User>(userList);

            // Return.
            return Ok(result);
        }

        [Authorize(Policy = "SuperuserPolicy")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            user.Id = id;

            // Act.
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);
            await _organizationManager.UpdateUserAsync(sessionOrganization.Id, user);

            // Return.
            return NoContent();
        }

        [Authorize(Policy = "SuperuserPolicy")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            // Act.
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);
            await _organizationManager.DeleteUserAsync(sessionOrganization.Id, id);

            // Return.
            return NoContent();
        }
    }
}
