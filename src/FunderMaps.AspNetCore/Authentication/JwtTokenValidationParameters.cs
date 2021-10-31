using Microsoft.IdentityModel.Tokens;

namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Extends the <see cref="TokenValidationParameters"/> with additional JWT parameters.
    /// </summary>
    public class JwtTokenValidationParameters : TokenValidationParameters
    {
        /// <summary>
        ///     Token valid time.
        /// </summary>
        public TimeSpan Valid { get; init; }
    }
}
