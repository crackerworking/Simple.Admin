using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain.User
{
    internal class CurrentUser : ICurrentUser
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsDemo => !IsSuperAdmin && UserName != null && UserName.Contains("demo");

        public IEnumerable<long> FuncIds { get; set; }

        public IEnumerable<string?> AuthCodes { get; set; }

        public CurrentUser()
        {
            UserId = -1;
            IsSuperAdmin = false;
        }
    }
}