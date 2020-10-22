using Microsoft.IdentityModel.Tokens;

namespace FunderMaps.AspNetCore.Authentication
{
    /// <summary>
    ///     Security token context.
    /// </summary>
    public class TokenContext
    {
        /// <summary>
        ///     Security token as string.
        /// </summary>
        public string TokenString { get; set; }

        /// <summary>
        ///     Security token.
        /// </summary>
        public SecurityToken Token { get; set; }
    }
}
