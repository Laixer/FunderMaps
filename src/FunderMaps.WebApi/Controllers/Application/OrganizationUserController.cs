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

namespace FunderMaps.WebApi.Controllers.Application
{
    [Authorize]
    [ApiController, Route("api/organization/user")]
    public class OrganizationUserController : BaseApiController
    {
        private static readonly Guid testOrgId = Guid.Parse("042c0b02-f387-4791-b87c-c13b16e963c8"); // TODO: Get from context

        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationUserController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            user = await _organizationManager.AddUserAsync(testOrgId, user);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] PaginationModel pagination)
        {
            // Assign.
            IAsyncEnumerable<User> userList = _organizationManager.GetAllUserAsync(testOrgId, pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<UserDto>, User>(userList);

            // Return.
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);
            user.Id = id;

            // Act.
            await _organizationManager.UpdateUserAsync(testOrgId, user);

            // Return.
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            // Act.
            await _organizationManager.DeleteUserAsync(testOrgId, id);

            // Return.
            return NoContent();
        }
    }
}
