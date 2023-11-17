using Microsoft.AspNetCore.Razor.TagHelpers;

using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.RazorLibrary.TagHelpers
{
    /// <summary>
    /// 权限按钮（按照权限控制是否显示）
    /// </summary>
    [HtmlTargetElement("permission-button")]
    public class PermissionButtonTagHelper : TagHelper
    {
        private readonly ICurrentUser _miUser;

        public PermissionButtonTagHelper(ICurrentUser miUser)
        {
            _miUser = miUser;
        }

        /// <summary>
        /// html属性id
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// html属性class
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// html属性lay-filter
        /// </summary>
        public string? LayFilter { get; set; }

        /// <summary>
        /// html属性lay-submit
        /// </summary>
        public string? LaySubmit { get; set; }

        /// <summary>
        /// html属性auth-code 权限code
        /// </summary>
        public string? AuthCode { get; set; }

        /// <summary>
        /// html属性lay-event
        /// </summary>
        public string? LayEvent { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";
            if (!string.IsNullOrEmpty(Id)) output.Attributes.SetAttribute("id", Id);
            if (!string.IsNullOrEmpty(ClassName)) output.Attributes.SetAttribute("class", ClassName);
            if (!string.IsNullOrEmpty(LayFilter)) output.Attributes.SetAttribute("lay-filter", LayFilter);
            if (!string.IsNullOrEmpty(LaySubmit)) output.Attributes.SetAttribute("lay-submit", "true");
            if (!string.IsNullOrEmpty(LayEvent)) output.Attributes.SetAttribute("lay-event", LayEvent);
            if (!string.IsNullOrEmpty(AuthCode))
            {
                // 不含权限，隐藏按钮
                if (!_miUser.AuthCodes.Any(x => x == AuthCode))
                {
                    output.SuppressOutput();
                    return;
                }
                output.Attributes.SetAttribute("auth-code", AuthCode);
            }

            base.Process(context, output);
        }
    }
}