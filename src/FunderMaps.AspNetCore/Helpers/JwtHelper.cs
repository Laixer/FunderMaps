using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FunderMaps.AspNetCore.Helpers
{
    // TODO This is a duplicate of FunderMaps.WebApi.Helpers.JwtHelper.cs
    /// <summary>
    ///     Helper class for jwt tokens.
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        ///     Create a new security key.
        /// </summary>
        /// <param name="defaultKey">The default key.</param>
        /// <returns><see cref="SecurityKey"/></returns>
        public static SecurityKey CreateSecurityKey(string defaultKey = null)
        {
            if (!string.IsNullOrEmpty(defaultKey))
            {
                return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(defaultKey));
            }

            // FUTURE: Write warning to log
            // FUTURE: Using IRandom
            using var rng = new Core.Components.RandomGenerator();
            return new SymmetricSecurityKey(rng.GetRandomByteArray(32));
        }
    }
}
