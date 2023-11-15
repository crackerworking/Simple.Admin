namespace Mi.Domain.Shared.Models
{
    public class UserModel
    {
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 角色，多个','拼接
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// 权限项
        /// </summary>
        public IList<PowerItem>? PowerItems { get; set; }

        public UserModel()
        {
            UserId = -1;
            UserName = "System";
            Roles = "";
        }
    }

    public class PowerItem
    {
        /// <summary>
        /// 功能ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string? AuthCode { get; set; }
    }
}