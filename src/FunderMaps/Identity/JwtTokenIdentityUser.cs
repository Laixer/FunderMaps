using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FunderMaps.Identity
{
    public class JwtTokenIdentityUser<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        /// <summary>
        /// List of identity claims.
        /// </summary>
        public List<Claim> Claims { get; }

        /// <summary>
        /// Signing credentials.
        /// </summary>
        public SigningCredentials SigningCredentials { get; }

        /// <summary>
        /// Token signature algorithm.
        /// </summary>
        public string SignatureAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

        /// <summary>
        /// Token issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Token validity (in minutes).
        /// </summary>
        public int TokenValid { get; set; } = 30;

        /// <summary>
        /// Create a new instance of the JWT token identity user.
        /// </summary>
        /// <param name="user">Identity user.</param>
        /// <param name="key">Symmetric key used for signatures.</param>
        public JwtTokenIdentityUser(TUser user, SymmetricSecurityKey key)
        {
            Claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            SigningCredentials = GetSigningKey(key);
        }

        /// <summary>
        /// Add principal roles as authorization claims.
        /// </summary>
        /// <param name="roles">List of user roles.</param>
        public void AddRoleClaims(IList<string> roles)
        {
            foreach (var role in roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
        }

        /// <summary>
        /// Add additional security claims.
        /// </summary>
        /// <param name="claims">Additional security claims.</param>
        public void AddClaims(IDictionary<string, string> claims)
        {
            foreach (var claim in claims)
            {
                Claims.Add(new Claim(claim.Key, claim.Value));
            }
        }

        /// <summary>
        /// Add single security claim.
        /// </summary>
        /// <param name="key">Claim key.</param>
        /// <param name="value">Claim value.</param>
        public void AddClaim(string key, object value)
        {
            Claims.Add(new Claim(key, value.ToString()));
        }

        /// <summary>
        /// Use symmetric key as token signature credentials.
        /// </summary>
        /// <param name="key">Symmetric key.</param>
        /// <returns>Signature credentials.</returns>
        protected SigningCredentials GetSigningKey(SymmetricSecurityKey key)
        {
            return new SigningCredentials(key, SignatureAlgorithm);
        }

        /// <summary>
        /// Create the JWT token.
        /// </summary>
        /// <param name="claims">List of authorization claims</param>
        /// <returns>Jwt token.</returns>
        protected JwtSecurityToken ConstructJwtToken(List<Claim> claims)
        {
            var tokenNotValidBefore = DateTime.Now;
            var tokenNotValidAfter = DateTime.Now.AddMinutes(TokenValid);

            return new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                notBefore: tokenNotValidBefore,
                expires: tokenNotValidAfter,
                signingCredentials: SigningCredentials);
        }

        /// <summary>
        /// Serialize jwt token into string.
        /// </summary>
        /// <returns>Token as string.</returns>
        public string WriteToken()
        {
            return handler.WriteToken(ConstructJwtToken(Claims));
        }
    }
}
