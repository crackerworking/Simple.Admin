using System.Diagnostics.CodeAnalysis;

namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class SysUserFull
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        public string? PasswordSalt { get; set; }

        /// <summary>
        /// 1超级管理员
        /// </summary>
        public int IsSuperAdmin { get; set; } = 0;

        /// <summary>
        /// 启用1 禁用0
        /// </summary>
        public int IsEnabled { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        public string? Signature { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }
    }
}