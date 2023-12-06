using FunderMaps.AspNetCore.Services;
using FunderMaps.Core.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace FunderMaps.AspNetCore.Authentication;

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
public class AuthKeyAuthenticationHandler(SignInService signInService, IOptionsMonitor<AuthKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : AuthenticationHandler<AuthKeyAuthenticationOptions>(options, logger, encoder)
{
    private readonly SignInService _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));

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

                var principal = await _signInService.AuthKeySignInAsync(token, Scheme.Name);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }

            var authQuery = Request.Query["authkey"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authQuery))
            {
                var principal = await _signInService.AuthKeySignInAsync(authQuery, Scheme.Name);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
            }

            return AuthenticateResult.Fail("Missing authorization header");
        }
        catch (AuthenticationException ex)
        {
            return AuthenticateResult.Fail(ex.Message);
        }
    }
}
