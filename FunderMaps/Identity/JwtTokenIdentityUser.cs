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

        public List<Claim> Claims { get; }
        public SigningCredentials SigningCredentials { get; }

        public string SignatureAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenValid { get; set; } = 30;

        public JwtTokenIdentityUser(TUser user, SymmetricSecurityKey key)
        {
            Claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            SigningCredentials = GetSigningKey(key);
        }

        protected SigningCredentials GetSigningKey(SymmetricSecurityKey key)
        {
            return new SigningCredentials(key, SignatureAlgorithm);
        }

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

        public string WriteToken()
        {
            return handler.WriteToken(ConstructJwtToken(Claims));
        }
    }
}
