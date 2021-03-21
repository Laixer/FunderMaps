﻿using AutoMapper;
using FunderMaps.AspNetCore.DataTransferObjects;
using FunderMaps.Core.Entities;
using FunderMaps.Core.Interfaces.Repositories;
using FunderMaps.Core.Types;
using FunderMaps.WebApi.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    public class ReviewerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly Core.AppContext _appContext;
        private readonly IOrganizationUserRepository _organizationUserRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        ///     Create new instance.
        /// </summary>
        public ReviewerController(IMapper mapper, IMemoryCache cache, Core.AppContext appContext, IOrganizationUserRepository organizationUserRepository, IUserRepository userRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
            // README: XXX: Response caching is a test

            var cacheKey = $"ReviewerDto{_appContext.TenantId}";

            // Fetch.
            if (!_cache.TryGetValue(cacheKey, out IList<ReviewerDto> result))
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
                result = _mapper.Map<IList<ReviewerDto>>(userList);

                // Set.
                _cache.Set(cacheKey, result, TimeSpan.FromHours(1));
            }

            // Return.
            return Ok(result);
        }
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
