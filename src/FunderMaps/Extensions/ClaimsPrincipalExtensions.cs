using FunderMaps.Core.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using ClaimTypes = FunderMaps.Authorization.ClaimTypes;

namespace FunderMaps.Extensions
{
    /// <summary>
    /// Claims principal extensions.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {

        /// <summary>
        /// Get the claim value by claim name.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="claimName">Claim key.</param>
        /// <returns>claim value.</returns>
        [Obsolete]
        public static string GetClaim(this ClaimsPrincipal principal, string claimName)
        {
            var claim = principal.Claims.Where(s => s.Type == claimName).FirstOrDefault();
            if (claim == null) { return null; }

            return claim.Value;
        }

        /// <summary>
        /// Check if user has organization.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>True on success, false otherwise.</returns>
        public static bool HasOrganization(this ClaimsPrincipal principal) =>
            principal.FindFirst(ClaimTypes.OrganizationUser) != null;

        /// <summary>
        /// Get the organization identifier.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>Organization identifier.</returns>
        public static Guid GetOrganizationId(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.OrganizationUser);
            if (claim == null)
            {
                throw new InvalidOperationException(); // TODO: Usefull exception
            }

            return Guid.Parse(claim.Value);
        }

        /// <summary>
        /// Get the organization role.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>Organization role.</returns>
        public static OrganizationRole GetOrganizationRole(this ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(ClaimTypes.OrganizationUserRole);
            if (claim == null)
            {
                throw new InvalidOperationException(); // TODO: Usefull exception
            }

            // TODO: Handle parse errors.
            return (OrganizationRole)Enum.Parse(typeof(OrganizationRole), claim.Value);
        }

        /// <summary>
        /// Check if organization user is in provided role.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <param name="role">Role to test.</param>
        /// <returns>True on success, false otherwise.</returns>
        public static bool IsInOrganizationRole(this ClaimsPrincipal principal, OrganizationRole role)
            => GetOrganizationRole(principal) == role;
    }
}
