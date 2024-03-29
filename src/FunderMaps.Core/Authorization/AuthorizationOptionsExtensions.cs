﻿using FunderMaps.Core.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FunderMaps.Core.Authorization;

/// <summary>
///     Extensions to the authorization options.
/// </summary>
public static class AuthorizationOptionsExtensions
{
    /// <summary>
    ///     Add FunderMaps authorization policy.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Consult the policy matrix before setting a authorization policy on anything.
    ///     </para>
    ///     <para>
    ///        Authorization policy matrix.
    ///
    ///                                      || ApplicationRole        || OrganizationRole
    ///                                      || Administrator   | User || Superuser | Verifier | Writer | Reader
    ///         AdministratorPolicy          || Yes             | No   || No        | No       | No     | No
    ///         SuperuserAdministratorPolicy || Yes             | No   || Yes       | No       | No     | No
    ///         SuperuserPolicy              || Yes             | Yes  || Yes       | No       | No     | No
    ///         VerifierAdministratorPolicy  || Yes             | No   || Yes       | Yes      | No     | No
    ///         VerifierPolicy               || Yes             | Yes  || Yes       | Yes      | No     | No
    ///         WriterAdministratorPolicy    || Yes             | No   || Yes       | Yes      | Yes    | No
    ///         WriterPolicy                 || Yes             | Yes  || Yes       | Yes      | Yes    | No
    ///         ReaderAdministratorPolicy    || Yes             | No   || Yes       | Yes      | Yes    | Yes
    ///         ReaderPolicy                 || Yes             | Yes  || Yes       | Yes      | Yes    | Yes
    ///     </para>
    /// </remarks>
    /// <param name="options">The authorization policy options.</param>
    public static void AddFunderMapsPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(AuthorizationPolicy.AdministratorPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireRole(Types.ApplicationRole.Administrator.ToString()));

        options.AddPolicy(AuthorizationPolicy.SuperuserAdministratorPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.IsInRole(Types.ApplicationRole.Administrator.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.SuperuserPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.VerifierAdministratorPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Verifier.ToString()) ||
                       context.User.IsInRole(Types.ApplicationRole.Administrator.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.VerifierPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Verifier.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.WriterAdministratorPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Verifier.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Writer.ToString()) ||
                       context.User.IsInRole(Types.ApplicationRole.Administrator.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.WriterPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Verifier.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Writer.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.ReaderAdministratorPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Verifier.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Writer.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Reader.ToString()) ||
                       context.User.IsInRole(Types.ApplicationRole.Administrator.ToString());
            }));

        options.AddPolicy(AuthorizationPolicy.ReaderPolicy, policy => policy
            .RequireAuthenticatedUser()
            .RequireAssertion(context =>
            {
                return context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Superuser.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Verifier.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Writer.ToString()) ||
                       context.User.HasClaim(FunderMapsAuthenticationClaimTypes.TenantRole, Types.OrganizationRole.Reader.ToString());
            }));
    }
}
