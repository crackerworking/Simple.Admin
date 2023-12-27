using Microsoft.AspNetCore.Mvc.Filters;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.ControllerLibrary;
using Simple.Admin.ControllerLibrary.Account;

namespace Simple.Admin.Web.Host.Filter
{
    public class GlobalActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<GlobalActionFilterAttribute> _logger;
        private readonly ILogService _logService;

        public GlobalActionFilterAttribute(ILogger<GlobalActionFilterAttribute> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }
    }
}