using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using FunderMaps.Helpers;

namespace FunderMaps.Authorization.Handler
{
    public class OrganizationMemberHandler : AuthorizationHandler<OrganizationMemberRequirement, Organization>
    {
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;

        public OrganizationMemberHandler(FunderMapsDbContext context, UserManager<FunderMapsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OrganizationMemberRequirement requirement,
            Organization organization)
        {
            // Authentication is required.
            if (context.User.Identity == null)
            {
                return;
            }

            var user = await _userManager.FindByEmailAsync(context.User.Identity.Name);

            // Administrator roles can access anything
            if (requirement.AllowAdministratorAlways && await IsAdministrator(user))
            {
                context.Succeed(requirement);
                return;
            }

            // Test if user is organization member
            if (await IsOrganizationMember(user, organization))
            {
                context.Succeed(requirement);
                return;
            }
        }

        /// <summary>
        /// Test if user claims the administrator role.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <returns>True on success.</returns>
        private async Task<bool> IsAdministrator(FunderMapsUser user)
        {
            return await _userManager.IsInRoleAsync(user, Constants.AdministratorRole);
        }

        /// <summary>
        /// Test if user belongs to organization as member.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <param name="organization">Organization resource.</param>
        /// <returns>True on success.</returns>
        private async Task<bool> IsOrganizationMember(FunderMapsUser user, Organization organization)
        {
            return await _context.OrganizationUsers.FindAsync(user.Id, organization.Id) != null;
        }
    }
}
