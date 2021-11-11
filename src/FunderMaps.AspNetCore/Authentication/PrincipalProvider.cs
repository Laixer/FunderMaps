using FunderMaps.Core.Identity;
using FunderMaps.Core.Types;
using System.Security.Claims;

namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Provides APIs for principal binder and unbinders.
    /// </summary>
    public static class PrincipalProvider
    {
        /// <summary>
        ///     Create <see cref="ClaimsIdentity"/> for specified <see cref="IUser"/>.
        /// </summary>
        /// <param name="user">The tenant user to create the principal for.</param>
        /// <param name="tenant">The tenant which the user is a member of.</param>
        /// <param name="tenantRole">The user role within the tenant.</param>
        /// <param name="authenticationType">Authentication type to use in authentication scheme.</param>
        /// <returns>Instance of <see cref="ClaimsIdentity"/>.</returns>
        public static ClaimsIdentity CreateTenantUserIdentity(
            IUser user,
            ITenant tenant,
            OrganizationRole tenantRole,
            string authenticationType)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (tenant is null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(FunderMapsAuthenticationClaimTypes.Tenant, tenant.Id.ToString()),
                new Claim(FunderMapsAuthenticationClaimTypes.TenantRole, tenantRole.ToString()),
            };

            return new(claims, authenticationType, ClaimTypes.Name, ClaimTypes.Role);
        }

        /// <summary>
        ///     Create <see cref="ClaimsPrincipal"/> for specified <see cref="IUser"/>.
        /// </summary>
        /// <param name="user">The tenant user to create the principal for.</param>
        /// <param name="tenant">The tenant which the user is a member of.</param>
        /// <param name="tenantRole">The user role within the tenant.</param>
        /// <param name="authenticationType">Authentication type to use in authentication scheme.</param>
        /// <param name="additionalClaims">Additional claims that will be stored in the claim.</param>
        /// <returns>Instance of <see cref="ClaimsPrincipal"/>.</returns>
        public static ClaimsPrincipal CreateTenantUserPrincipal(
            IUser user,
            ITenant tenant,
            OrganizationRole tenantRole,
            string authenticationType,
            IEnumerable<Claim> additionalClaims = null)
        {
            ClaimsIdentity identity = CreateTenantUserIdentity(user, tenant, tenantRole, authenticationType);

            if (additionalClaims is not null)
            {
                foreach (var claim in additionalClaims)
                {
                    identity.AddClaim(claim);
                }
            }

            return new(identity);
        }

        /// <summary>
        ///     Returns true if the principal has an identity with the application identity.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
        /// <returns><c>True</c> if the user is logged in with identity.</returns>
        public static bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return principal.Identities.Any(i => i.IsAuthenticated);
        }

        /// <summary>
        ///     Returns the <see cref="IUser"/> and <see cref="ITenant"/> from the principal.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> instance.</param>
        /// <returns>Tuple of <see cref="IUser"/> and <see cref="ITenant"/>.</returns>
        public static (IUser, ITenant) GetUserAndTenant<TUser, TTenant>(ClaimsPrincipal principal)
            where TUser : IUser, new()
            where TTenant : ITenant, new()
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            string GetValueOrThrow(string type)
            {
                var idClaim = principal.FindFirst(type);
                if (idClaim == null)
                {
                    throw new InvalidOperationException();
                }

                return idClaim.Value;
            }

            TUser user = new()
            {
                Id = Guid.Parse(GetValueOrThrow(ClaimTypes.NameIdentifier)),
                Role = (ApplicationRole)Enum.Parse(typeof(ApplicationRole), GetValueOrThrow(ClaimTypes.Role)),
            };

            TTenant tenant = new()
            {
                Id = Guid.Parse(GetValueOrThrow(FunderMapsAuthenticationClaimTypes.Tenant)),
            };

            return (user, tenant);
        }
    }
}
