namespace Simple.Admin.Domain.Shared.Core
{
    public interface ICurrentUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        long UserId { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        bool IsSuperAdmin { get; }

        /// <summary>
        /// 是否演示用户
        /// </summary>
        bool IsDemo { get; }

        /// <summary>
        /// 所有功能ID
        /// </summary>
        IEnumerable<long> FuncIds { get; }

        /// <summary>
        /// 所有权限code
        /// </summary>
        IEnumerable<string?> AuthCodes { get; }
    }
}