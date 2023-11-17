using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Simple.Admin.Application.Contracts.System.Models.Log
{
    public class SysLoginLogFull
    {
        /// <summary>
        /// 登录用户名
        /// </summary>
        [NotNull]
        public string? UserName { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string? System { get; set; }

        /// <summary>
        /// 1成功 0失败
        /// </summary>
        [DefaultValue(0)]
        public int Status { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        public string? OperationInfo { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// 区域信息
        /// </summary>
        public string? RegionInfo { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}