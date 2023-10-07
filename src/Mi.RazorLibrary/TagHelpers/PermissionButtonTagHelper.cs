using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Mi.RazorLibrary.TagHelpers
{
    [HtmlTargetElement("permission-button")]
    public class PermissionButtonTagHelper : TagHelper
    {
        private readonly ICurrentUser _miUser;

        public PermissionButtonTagHelper(ICurrentUser miUser)
        {
            _miUser = miUser;
        }

        public string? Id { get; set; }

        public string? ClassName { get; set; }

        public string? LayFilter { get; set; }

        public string? LaySubmit { get; set; }

        public string? AuthCode { get; set; }

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