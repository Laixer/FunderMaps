using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.WebApi.DataTransferObjects;
using FunderMaps.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#pragma warning disable CA1062 // Validate arguments of public methods
namespace FunderMaps.WebApi.Controllers.Application
{
    /// <summary>
    ///     Endpoint controller for organization user administration.
    /// </summary>
    /// <remarks>
    ///     This controller provides organization administration.
    ///     <para>
    ///         For the variant based on the current session see 
    ///         <see cref="OrganizationUserController"/>.
    ///     </para>
    /// </remarks>
    [Authorize(Policy = "AdministratorPolicy")]
    [Route("admin/organization/{id:guid}/user")]
    public class OrganizationUserAdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserAdminController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(Guid id, [FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            user = await _organizationManager.AddUserAsync(id, user);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync(Guid id, [FromQuery] PaginationModel pagination)
        {
            // Assign.
            IAsyncEnumerable<User> userList = _organizationManager.GetAllUserAsync(id, pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<UserDto>, User>(userList);

            // Return.
            return Ok(result);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, Guid userId, [FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            user.Id = userId;

            // Act.
            await _organizationManager.UpdateUserAsync(id, user);

            // Return.
            return NoContent();
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, Guid userId)
        {
            // Act.
            await _organizationManager.DeleteUserAsync(id, userId);

            // Return.
            return NoContent();
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
