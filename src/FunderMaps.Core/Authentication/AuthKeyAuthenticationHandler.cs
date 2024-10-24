using FunderMaps.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Text.Encodings.Web;

namespace FunderMaps.Core.Authentication;

public class AuthKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "AuthKeyAuthenticationScheme";
}

/// <summary>
///     Authentication handler for auth key authentication.
/// </summary>
/// <remarks>
///     Create the auth key authentication handler.
/// </remarks>
public class AuthKeyAuthenticationHandler(
    SignInService signInService,
    IOptionsMonitor<AuthKeyAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder) : AuthenticationHandler<AuthKeyAuthenticationOptions>(options, logger, encoder)
{
    /// <summary>
    ///     Authenticate the request.
    /// </summary>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            var authHeader = Request.Headers.Authorization.FirstOrDefault();
            if (authHeader?.StartsWith("authkey ", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                var token = authHeader.Trim()["authkey ".Length..].Trim();

                var principal = await signInService.AuthKeySignInAsync(token, Scheme.Name);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }

            var authQuery = Request.Query["authkey"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authQuery))
            {
                var principal = await signInService.AuthKeySignInAsync(authQuery, Scheme.Name);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }

            var apiKeyHeader = Request.Headers["X-API-Key"].FirstOrDefault();
            if (!string.IsNullOrEmpty(apiKeyHeader))
            {
                var principal = await signInService.AuthKeySignInAsync(apiKeyHeader, Scheme.Name);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }

            return AuthenticateResult.NoResult();
        }
        catch (AuthenticationException ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }
}
