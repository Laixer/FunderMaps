using AutoMapper;
using FunderMaps.Controllers;
using FunderMaps.Core.Authentication;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Managers;
using FunderMaps.Core.Types;
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
    ///     Endpoint controller for application reviewers.
    /// </summary>
    /// <remarks>
    ///     This controller should *only* handle operations on the current
    ///     user session. Therefore the user context must be active.
    /// </remarks>
    [Authorize(Policy = "WriterPolicy")]
    public class ReviewerController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly AuthManager _authManager;
        private readonly OrganizationManager _organizationManager;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ReviewerController(IMapper mapper, AuthManager authManager, OrganizationManager organizationManager)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _authManager = authManager ?? throw new ArgumentNullException(nameof(authManager));
            _organizationManager = organizationManager ?? throw new ArgumentNullException(nameof(organizationManager));
        }

        // GET: api/reviewer
        /// <summary>
        ///     Return all reviewers.
        /// </summary>
        /// <remarks>
        ///     Cache response for 1 hour.
        /// </remarks>
        [HttpGet("reviewer"), ResponseCache(Duration = 60 * 60)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationModel pagination)
        {
            // Assign.
            var roles = new OrganizationRole[]{ OrganizationRole.Verifier, OrganizationRole.Superuser };
            Organization sessionOrganization = await _authManager.GetOrganizationAsync(User);
            IAsyncEnumerable<User> userList = _organizationManager.GetAllUserByRoleAsync(sessionOrganization.Id, roles, pagination.Navigation);

            // Map.
            var result = await _mapper.MapAsync<IList<ReviewerDto>, User>(userList);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
