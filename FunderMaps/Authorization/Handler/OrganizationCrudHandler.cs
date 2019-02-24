using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FunderMaps.Data;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using FunderMaps.Authorization.Requirement;
using FunderMaps.Helpers;

namespace FunderMaps.Authorization.Handler
{
    public class OrganizationCrudHandler : OrganizationHandler<OperationAuthorizationRequirement>
    {
        private readonly ILookupNormalizer _keyNormalizer;

        public OrganizationCrudHandler(FunderMapsDbContext context,
            UserManager<FunderMapsUser> userManager,
            ILookupNormalizer keyNormalizer)
            : base(context, userManager)
        {
            _keyNormalizer = keyNormalizer;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
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
            //var organizationUser = await Context.OrganizationUsers
            //    .Include(s => s.Organization)
            //    .Include(s => s.OrganizationRole)
            //    .Select(s => new { s.UserId, s.Organization, s.OrganizationRole })
            //    .SingleOrDefaultAsync(q => q.UserId == user.Id);

            // Administrator roles can access anything
            if (await IsAdministratorAsync(user))
            {
                context.Succeed(requirement);
                return;
            }

            if (requirement.Name == OperationsRequirement.Read.Name)
            {
                if (await HasOrganizationRoleAsync(user, organization, new[] {
                    Constants.ReaderRole,
                    Constants.WriterRole,
                    Constants.VerifierRole,
                    Constants.SuperuserRole
                }))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            else if (requirement.Name == OperationsRequirement.Create.Name || requirement.Name == OperationsRequirement.Update.Name)
            {
                if (await HasOrganizationRoleAsync(user, organization, new[] {
                    Constants.WriterRole,
                    Constants.VerifierRole,
                    Constants.SuperuserRole
                }))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            else if (requirement.Name == OperationsRequirement.Delete.Name)
            {
                if (await HasOrganizationRoleAsync(user, organization, new[] {
                    Constants.SuperuserRole
                }))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
            else if (requirement.Name == OperationsRequirement.Validate.Name)
            {
                if (await HasOrganizationRoleAsync(user, organization, new[] {
                    Constants.VerifierRole,
                    Constants.SuperuserRole
                }))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }

        /// <summary>
        /// Test if user belongs to organization as member.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <param name="organization">Organization resource.</param>
        /// <param name="role">Organization role.</param>
        /// <returns>True on success.</returns>
        protected async Task<bool> HasOrganizationRoleAsync(FunderMapsUser user, Organization organization, string[] role)
        {
            role = role.Select(r => _keyNormalizer.Normalize(r)).ToArray();

            return await Context.OrganizationUsers
                .AsNoTracking()
                .Include(e => e.OrganizationRole)
                .Where(s => role.Contains(s.OrganizationRole.NormalizedName))
                .FirstOrDefaultAsync(s => s.User.Id == user.Id && s.Organization.Id == organization.Id) != null;
        }
    }
}
