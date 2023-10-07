using Mi.Domain.Shared.Core;

namespace Mi.Domain.User
{
    internal class CurrentUser : ICurrentUser
    {
        public long UserId { get; set; }

        public string UserName { get; set; }

        public bool IsSuperAdmin { get; set; }

        public IEnumerable<long> FuncIds { get; set; }

        public IEnumerable<string?> AuthCodes { get; set; }

        public CurrentUser()
        {
            UserId = -1;
            IsSuperAdmin = false;
        }
    }
}