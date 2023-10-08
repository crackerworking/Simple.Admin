using System.Linq.Expressions;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mi.RazorLibrary
{
    public static class HtmlExtension
    {
        public static IHtmlContent HiddenInput<TModel, TResult>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TResult>> expression)
        {
            return new HtmlString($"<input name='' value='' type='hidden' />");
        }
    }
}