using FunderMaps.AspNetCore.Services;
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
public class AuthKeyAuthenticationHandler : AuthenticationHandler<AuthKeyAuthenticationOptions>
{
    private readonly SignInService _signInService;

    /// <summary>
    ///     Create the auth key authentication handler.
    /// </summary>
    public AuthKeyAuthenticationHandler(SignInService signInService, IOptionsMonitor<AuthKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
        _signInService = signInService ?? throw new ArgumentNullException(nameof(signInService));
    }

    /// <summary>
    ///     Authenticate the request.
    /// </summary>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (authHeader?.StartsWith("authkey ", StringComparison.InvariantCultureIgnoreCase) ?? false)
        {
            var token = authHeader.Trim().Substring("authkey ".Length).Trim();

            var principal = await _signInService.AuthKeySignInAsync(token, this.Scheme.Name);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, this.Scheme.Name));
        }

        var authQuery = Request.Query["authkey"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authQuery))
        {
            var principal = await _signInService.AuthKeySignInAsync(authQuery, this.Scheme.Name);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, this.Scheme.Name));
        }

        return AuthenticateResult.Fail("Missing authorization header");
    }
}
