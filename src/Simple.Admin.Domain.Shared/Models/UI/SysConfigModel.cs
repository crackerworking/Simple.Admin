using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Simple.Admin.Domain.Shared.Models.UI
{
    public class SysConfigModel
    {
        /// <summary>
        /// 站点标题
        /// </summary>
        [Description("站点标题")]
        public string? site_title { get; set; }

        /// <summary>
        /// 站点ico
        /// </summary>
        [Description("站点ico")]
        public string? site_icon { get; set; }

        /// <summary>
        /// 进入后侧边栏Logo
        /// </summary>
        [Description("进入后侧边栏Logo")]
        public string? logo { get; set; }

        /// <summary>
        /// 进入后侧边栏显示名称
        /// </summary>
        [NotNull]
        [Required]
        [Description("进入后侧边栏显示名称")]
        public string? header_name { get; set; }

        /// <summary>
        /// 登录页中间显示名称
        /// </summary>
        [Description("登录页中间显示名称")]
        public string? login_middle_name { get; set; }

        /// <summary>
        /// 登录页底部显示文本
        /// </summary>
        [Description("登录页底部显示文本")]
        public string? login_footer_word { get; set; }

        /// <summary>
        /// 首页名称
        /// </summary>
        [NotNull]
        [Required]
        [Description("首页名称")]
        public string? home_page_name { get; set; }

        /// <summary>
        /// 首页地址
        /// </summary>
        [NotNull]
        [Description("首页地址")]
        public string? home_page_url { get; set; }
    }
}