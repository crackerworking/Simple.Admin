using System.Diagnostics.CodeAnalysis;

namespace Mi.Domain.Shared.Models
{
    public class MiHeader
    {
        /// <summary>
        /// ip地址
        /// </summary>
        [NotNull]
        public string? Ip { get; set; }

        /// <summary>
        /// 国家-省-市
        /// </summary>
        [NotNull]
        public string? Region { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string? System { get; set; }

        public MiHeader()
        {
            Region = "未知";
            Ip = "";
        }

        /// <summary>
        /// HttpContext.Items:key名
        /// </summary>
        public readonly static string MIHEADER = "Mi-Header";
    }
}
