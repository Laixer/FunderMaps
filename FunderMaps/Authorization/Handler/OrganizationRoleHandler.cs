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

namespace FunderMaps.Authorization.Handler
{
    public class OrganizationRoleHandler : OrganizationHandler<OrganizationRoleRequirement>
    {
        private readonly ILookupNormalizer _keyNormalizer;

        public OrganizationRoleHandler(FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            ILookupNormalizer keyNormalizer)
            : base(context, userManager)
        {
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
            if (organization == null)
            {
                return;
            }

            var user = await UserManager.FindByEmailAsync(context.User.Identity.Name);

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
            return await Context.OrganizationUsers
                .AsNoTracking()
                .Include(e => e.OrganizationRole)
                .Where(s => s.OrganizationRole.NormalizedName == _keyNormalizer.Normalize(role))
                .FirstOrDefaultAsync(s => s.User.Id == user.Id && s.Organization.Id == organization.Id) != null;
        }
    }
}
