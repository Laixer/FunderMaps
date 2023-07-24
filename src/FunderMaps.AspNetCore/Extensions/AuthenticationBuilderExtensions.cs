using FunderMaps.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FunderMaps.Extensions;

/// <summary>
///     Extensions to the authentication builder.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    /// <summary>
    ///     Add FunderMaps authentication scheme.
    /// </summary>
    public static AuthenticationBuilder AddFunderMapsScheme(this AuthenticationBuilder builder)
    {
        return builder.AddPolicyScheme("FunderMapsHybridAuth", "Bearer or AuthKey", options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
                if (authHeader?.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase) ?? false)
                {
                    return JwtBearerDefaults.AuthenticationScheme;
                }
                else if (authHeader?.StartsWith("AuthKey ", StringComparison.InvariantCultureIgnoreCase) ?? false || !string.IsNullOrEmpty(context.Request.Query["authkey"].FirstOrDefault()))
                {
                    return AuthKeyAuthenticationOptions.DefaultScheme;
                }
                return CookieAuthenticationDefaults.AuthenticationScheme;
            };
        });
    }
}
