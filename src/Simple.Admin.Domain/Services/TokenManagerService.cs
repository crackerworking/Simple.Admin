using System.Collections.Concurrent;
using System.Security.Claims;

using Simple.Admin.Domain.Helper;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Models;

namespace Simple.Admin.Domain.Services
{
    internal class TokenManagerService : ITokenManager
    {
        private readonly ConcurrentDictionary<string, TokenInfo> _infos;

        public TokenManagerService()
        {
            _infos = new ConcurrentDictionary<string, TokenInfo>();
        }

        public void AddToken(string token, DateTime expire)
        {
            var info = new TokenInfo { expire = expire, token = token };
            _infos.TryAdd(token, info);
        }

        public ClaimsPrincipal? GetClaimsPrincipal(string token)
        {
            return TokenHelper.Instance.GetPrincipalFromAccessToken(token);
        }

        public bool IsExpired(string token)
        {
            return _infos.TryGetValue(token, out var info) && info.expire <= DateTime.Now;
        }
    }
}