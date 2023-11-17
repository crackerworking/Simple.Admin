using Simple.Admin.Domain.Exceptions;

using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain.Helper
{
    /// <summary>
    /// 演示
    /// </summary>
    public class Demo
    {
        public static string Tip => "演示账号没有此权限";

        public static void ThrowExceptionForDBWriteAction(ICurrentUser currentUser)
        {
            if (currentUser.IsDemo)
            {
                throw new FriendlyException(Tip);
            }
        }
    }
}