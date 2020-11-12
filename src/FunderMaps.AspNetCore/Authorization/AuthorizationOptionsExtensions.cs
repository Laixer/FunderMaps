using FunderMaps.Core.Authentication;
using Microsoft.AspNetCore.Authorization;
using System;

namespace FunderMaps.AspNetCore.Authorization
{
    /// <summary>
    ///     Extensions to the authorization options.
    /// </summary>
    public static class AuthorizationOptionsExtensions
    {
        // FUTURE: Move the business logic to the core.
        /// <summary>
        ///     Add FunderMaps authorization policy.
        /// </summary>
        /// <remarks>
        ///     Authorization policy matrix
        ///     <para>
        ///                                      || ApplicationRole                || OrganizationRole
        ///                                      || Administrator   | User | Guest || Superuser | Verifier | Writer | Reader
        ///         AdministratorPolicy          || Yes             | No   | No    || No        | No       | No     | No
        ///         SuperuserAdministratorPolicy || Yes             | No   | No    || Yes       | No       | No     | No
        ///         SuperuserPolicy              || Yes             | Yes  | Yes   || Yes       | No       | No     | No
        ///         VerifierAdministratorPolicy  || Yes             | No   | No    || Yes       | Yes      | No     | No
        ///         VerifierPolicy               || Yes             | Yes  | Yes   || Yes       | Yes      | No     | No
        ///         WriterAdministratorPolicy    || Yes             | No   | No    || Yes       | Yes      | Yes    | No
        ///         WriterPolicy                 || Yes             | Yes  | Yes   || Yes       | Yes      | Yes    | No
        ///         ReaderAdministratorPolicy    || Yes             | No   | No    || Yes       | Yes      | Yes    | Yes
        ///         ReaderPolicy                 || Yes             | Yes  | Yes   || Yes       | Yes      | Yes    | Yes
        ///     </para>
        /// </remarks>
        /// <param name="options">The authorization policy options.</param>
        public static void AddFunderMapsPolicy(this AuthorizationOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.AddPolicy(AuthorizationPolicy.AdministratorPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireRole(Core.Types.ApplicationRole.Administrator.ToString()));

            options.AddPolicy(AuthorizationPolicy.SuperuserAdministratorPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.SuperuserPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.VerifierAdministratorPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                           context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.VerifierPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Verifier.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.WriterAdministratorPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Writer.ToString()) ||
                           context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.WriterPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Writer.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.ReaderAdministratorPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Writer.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Reader.ToString()) ||
                           context.User.IsInRole(Core.Types.ApplicationRole.Administrator.ToString());
                }));

            options.AddPolicy(AuthorizationPolicy.ReaderPolicy, policy => policy
                .RequireAuthenticatedUser()
                .RequireAssertion(context =>
                {
                    return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Superuser.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Verifier.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Writer.ToString()) ||
                           context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Core.Types.OrganizationRole.Reader.ToString());
                }));
        }
    }
}
