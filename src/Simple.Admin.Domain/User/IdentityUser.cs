using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using Simple.Admin.Domain.Shared;

namespace Simple.Admin.Domain.User
{
    public class IdentityUser
    {
        public static string CreateToken(Claim[] claims, int hour = 3)
        {
            //现在，是时候定义 jwt token 了，它将负责创建我们的 tokens
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // 从 appsettings 中获得我们的 secret
            var key = Encoding.ASCII.GetBytes(App.Configuration["JwtConfig:Secret"]!);

            // 定义我们的 token descriptor
            // 我们需要使用 claims （token 中的属性）给出关于 token 的信息，它们属于特定的用户，
            // 因此，可以包含用户的 Id、名字、邮箱等。
            // 好消息是，这些信息由我们的服务器和 Identity framework 生成，它们是有效且可信的。
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // token 的过期时间需要缩短，并利用 refresh token 来保持用户的登录状态，
                // 不过由于这只是一个演示应用，我们可以对其进行延长以适应我们当前的需求
                Expires = DateTime.UtcNow.AddHours(hour),
                // 这里我们添加了加密算法信息，用于加密我们的 token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}