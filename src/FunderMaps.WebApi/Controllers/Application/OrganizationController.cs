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
    ///     Endpoint controller for organization operations.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle organization operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [ApiController, Route("api/organization")]
    public class OrganizationController : BaseApiController
    {
        private static readonly Guid testOrgId = Guid.Parse("042c0b02-f387-4791-b87c-c13b16e963c8"); // TODO: Get from context

        private readonly IMapper _mapper;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public OrganizationController(IMapper mapper, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        #region Organization

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // Act.
            Organization organization = await _organizationManager.GetAsync(testOrgId).ConfigureAwait(false);

            // Map.
            var output = _mapper.Map<OrganizationDto>(organization);

            // Return.
            return Ok(output);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] OrganizationDto input)
        {
            // Map.
            var organization = _mapper.Map<Organization>(input);
            organization.Id = testOrgId;

            // Act.
            await _organizationManager.UpdateAsync(organization).ConfigureAwait(false);

            // Return.
            return NoContent();
        }

        #endregion Organization

        #region Organization User

        [HttpPost("user")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto input)
        {
            // Map.
            var user = _mapper.Map<User>(input);

            // Act.
            user = await _organizationManager.AddUserAsync(testOrgId, user).ConfigureAwait(false);

            // Map.
            var output = _mapper.Map<UserDto>(user);

            // Return.
            return Ok(output);
        }

        [HttpDelete("user/{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            // Act.
            await _organizationManager.DeleteUserAsync(testOrgId, id).ConfigureAwait(false);

            // Return.
            return NoContent();
        }

        #endregion Organization User
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
