using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.ControllerLibrary;
using Simple.Admin.ControllerLibrary.Account;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Options;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Web.Host.Filter
{
    public class GlobalActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<GlobalActionFilterAttribute> _logger;
        private readonly ILogService _logService;

        private static readonly string?[] IGNORE_CONTROLLERS = new string?[2] { typeof(PublicController).FullName,
            typeof(LoginController).FullName };

        public GlobalActionFilterAttribute(ILogger<GlobalActionFilterAttribute> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = context.ModelState.Keys.Where(key => context.ModelState[key] != null).Select(key => new Option { Name = key, Value = context.ModelState[key]!.Errors.FirstOrDefault()?.ErrorMessage });
                if (result != null)
                {
                    var msg = result.Where(x => !string.IsNullOrEmpty(x.Value)).FirstOrDefault()?.Value;
                    context.Result = new ObjectResult(new MessageModel(response_type.ParameterError, msg));
                    _logger.LogWarning($"请求地址：{context.HttpContext.Request.Path}，参数验证错误：{msg}");
                }
            }
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var enabled = Convert.ToBoolean(App.Configuration["ActionLog"]);
            if (!enabled) goto completed;
            if (!IGNORE_CONTROLLERS.Contains(context.Controller.ToString()))
            {
                var httpContext = context.HttpContext;
                var url = $"{{ \"http_verb\" : \"{httpContext.Request.Method}\" , \"path\" : \"{httpContext.Request.Path}\" }}";
                string? param = httpContext.Request.QueryString.Value;
                if (httpContext.Request.ContentType == "application/json")
                {
                    httpContext.Request.Body.Position = 0;
                    using var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                    param = await reader.ReadToEndAsync();
                }
                var guid = Guid.NewGuid().ToString();
                httpContext.Items.Add("RequestId", guid);
                await _logService.WriteLogAsync(url, param ?? "", context.ActionDescriptor.DisplayName, guid, httpContext.Request.ContentType, true);
            }

        completed:
            await next();
        }
    }
}