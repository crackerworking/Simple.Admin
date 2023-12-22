using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using Simple.Admin.Domain.Shared;

namespace Simple.Admin.Domain.Helper
{
    public class TokenHelper
    {
        private static Lazy<TokenHelper> _lazy => new(() => new TokenHelper());

        public static TokenHelper Instance => _lazy.Value;

        public static string GenerateToken(IEnumerable<Claim> claims, DateTime expireTime)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(App.Configuration.GetSection("JWT")["IssuerSigningKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                issuer: App.Configuration.GetSection("JWT")["ValidIssuer"],
                audience: App.Configuration.GetSection("JWT")["ValidAudience"],
                claims: claims,
                expires: expireTime,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}