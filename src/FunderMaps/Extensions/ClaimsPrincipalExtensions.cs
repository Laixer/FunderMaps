using FunderMaps.Core.Entities;
using FunderMaps.Exceptions;
using System;
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
        /// Check if user has organization.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>True on success, false otherwise.</returns>
        public static bool HasOrganization(this ClaimsPrincipal principal) =>
            principal?.FindFirst(ClaimTypes.OrganizationUser) != null;

        /// <summary>
        /// Get the organization identifier.
        /// </summary>
        /// <param name="principal">See <see cref="ClaimsPrincipal"/>.</param>
        /// <returns>Organization identifier.</returns>
        public static Guid GetOrganizationId(this ClaimsPrincipal principal)
        {
            var claim = principal?.FindFirst(ClaimTypes.OrganizationUser);
            if (claim == null)
            {
                throw new ClaimNotFoundException(ClaimTypes.OrganizationUser);
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
            var claim = principal?.FindFirst(ClaimTypes.OrganizationUserRole);
            if (claim == null)
            {
                throw new ClaimNotFoundException(ClaimTypes.OrganizationUser);
            }

            if (!Enum.TryParse(claim.Value, true, out OrganizationRole organizationRole))
            {
                throw new InvalidCastException($"Cannot cast value into {nameof(OrganizationRole)}");
            }

            return organizationRole;
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
