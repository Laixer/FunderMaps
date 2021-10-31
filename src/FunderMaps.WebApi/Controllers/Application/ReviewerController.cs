using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public class ReviewerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Core.AppContext _appContext;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ReviewerController(IMapper mapper, Core.AppContext appContext, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _organizationUserRepository = organizationUserRepository ?? throw new ArgumentNullException(nameof(organizationUserRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        // GET: api/reviewer
        /// <summary>
        ///     Return all reviewers.
        /// </summary>
        [HttpGet("reviewer"), ResponseCache(Duration = 60 * 60, VaryByHeader = "Authorization", Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetAllAsync([FromQuery] PaginationDto pagination)
        {
            // Act.
            // TODO: Single call
            var userList = new List<User>();
            var roles = new OrganizationRole[] { OrganizationRole.Verifier, OrganizationRole.Superuser };
            await foreach (var user in _organizationUserRepository.ListAllByRoleAsync(_appContext.TenantId, roles, pagination.Navigation))
            {
                userList.Add(await _userRepository.GetByIdAsync(user));
            }

            // Map.
            var result = _mapper.Map<IList<ReviewerDto>>(userList);

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
