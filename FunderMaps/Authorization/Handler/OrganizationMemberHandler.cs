using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;

namespace FunderMaps.Authorization.Handler
{
    public class OrganizationMemberHandler : OrganizationHandler<OrganizationMemberRequirement>
    {
        public OrganizationMemberHandler(FunderMapsDbContext context, UserManager<FunderMapsUser> userManager)
            : base(context, userManager)
        {
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
            if (await IsOrganizationMemberAsync(user, organization))
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
        /// <returns>True on success.</returns>
        private async Task<bool> IsOrganizationMemberAsync(FunderMapsUser user, Organization organization)
        {
            return await Context.OrganizationUsers.FirstOrDefaultAsync(s => s.UserId == user.Id && s.OrganizationId == organization.Id) != null;
        }
    }
}
