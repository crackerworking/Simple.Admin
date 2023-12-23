using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class UserItem
    {
        public long Id { get; set; }
        public long UserId => Id;

        [NotNull]
        public string? UserName { get; set; }

        [JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string? RoleIdStr { get; set; }

        public string[] RoleIds
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(RoleIdStr))
                {
                    return RoleIdStr.Split(',', StringSplitOptions.RemoveEmptyEntries);
                }
                return Array.Empty<string>();
            }
        }

        public DateTime CreatedOn { get; set; }

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