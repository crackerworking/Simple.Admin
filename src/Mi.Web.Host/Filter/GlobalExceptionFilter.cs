using System.Text;

using Mi.Application.Contracts.System;
using Mi.Domain.Exceptions;
using Mi.Domain.Extension;
using Mi.Domain.Helper;
using Mi.Domain.Shared;
using Mi.Domain.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Web.Host.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogService _logService;

        public GlobalExceptionFilter(ILogService logService)
        {
            _logService = logService;
        }

        public async void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                if (context.Exception is FriendlyException)
                {
                    context.Result = new ObjectResult(Back.Fail(context.Exception.Message));
                }
                else
                {
                    context.Result = new ObjectResult(new MessageModel(response_type.Error, context.Exception.Message));
                }
                var req = await LogParamsAsync(context.HttpContext);
                FileLogging.Instance.WriteException(context.Exception, req);
                if (context.HttpContext.Items.TryGetValue("RequestId", out var temp))
                {
                    var enabled = Convert.ToBoolean(App.Configuration["ActionLog"]);
                    if (enabled)
                    {
                        var guid = (string?)temp;
                        _logService.SetExceptionAsync(guid ?? Guid.NewGuid().ToString(), context.Exception.Message).ConfigureAwait(true).GetAwaiter();
                    }
                }
                context.ExceptionHandled = true;
            }
        }

        private static async Task<string> LogParamsAsync(HttpContext httpContext)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{httpContext.Request.Method}] -- {httpContext.Request.Path}");
            string? param = httpContext.Request.QueryString.Value;
            if (httpContext.Request.ContentType == "application/json")
            {
                httpContext.Request.Body.Position = 0;
                using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                param = await reader.ReadToEndAsync();
            }
            if (!param.IsNull())
            {
                sb.AppendLine("<!-- request params -->");
                sb.Append(param);
            }
            return sb.ToString();
        }
    }
}