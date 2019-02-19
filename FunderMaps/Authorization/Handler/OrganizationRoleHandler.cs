using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using FunderMaps.Helpers;

namespace FunderMaps.Authorization.Handler
{
    public class OrganizationRoleHandler : AuthorizationHandler<OrganizationRoleRequirement, Organization>
    {
        private readonly FunderMapsDbContext _context;
        private readonly UserManager<FunderMapsUser> _userManager;
        private readonly ILookupNormalizer _keyNormalizer;

        public OrganizationRoleHandler(FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _userManager = userManager;
            _keyNormalizer = keyNormalizer;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OrganizationRoleRequirement requirement,
            Organization organization)
        {
            // Authentication is required.
            if (context.User.Identity == null)
            {
                return;
            }

            var user = await _userManager.FindByEmailAsync(context.User.Identity.Name);

            // Administrator roles can access anything
            if (requirement.AllowAdministratorAlways && await IsAdministratorAsync(user))
            {
                context.Succeed(requirement);
                return;
            }

            // Test if user is organization member
            if (await HasOrganizationRoleAsync(user, organization, requirement.Role))
            {
                context.Succeed(requirement);
                return;
            }

            await Task.FromResult(0);
        }

        /// <summary>
        /// Test if user claims the administrator role.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <returns>True on success.</returns>
        private async Task<bool> IsAdministratorAsync(FunderMapsUser user)
        {
            return await _userManager.IsInRoleAsync(user, Constants.AdministratorRole);
        }

        /// <summary>
        /// Test if user belongs to organization as member.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <param name="organization">Organization resource.</param>
        /// <param name="role">Organization role.</param>
        /// <returns>True on success.</returns>
        private async Task<bool> HasOrganizationRoleAsync(FunderMapsUser user, Organization organization, string role)
        {
            return await _context.OrganizationUsers
                .Include(e => e.OrganizationRole)
                .Where(s => s.User.Id == user.Id &&
                    s.Organization.Id == organization.Id &&
                    s.OrganizationRole.NormalizedName == _keyNormalizer.Normalize(role))
                .FirstOrDefaultAsync() != null;
        }
    }
}
