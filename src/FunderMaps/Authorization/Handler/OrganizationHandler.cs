using FunderMaps.Data;
using FunderMaps.Helpers;
using FunderMaps.Models;
using FunderMaps.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FunderMaps.Authorization.Handler
{
    public abstract class OrganizationHandler<TRequirement> : AuthorizationHandler<TRequirement, Organization>
        where TRequirement : IAuthorizationRequirement
    {
        protected FunderMapsDbContext Context { get; }
        protected UserManager<FunderMapsUser> UserManager { get; }

        public OrganizationHandler(FunderMapsDbContext context, UserManager<FunderMapsUser> userManager)
        {
            Context = context;
            UserManager = userManager;
        }

        /// <summary>
        /// Test if user claims the administrator role.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <returns>True on success.</returns>
        protected async Task<bool> IsAdministratorAsync(FunderMapsUser user)
        {
            return await UserManager.IsInRoleAsync(user, Constants.AdministratorRole);
        }
    }
}
