using Mi.Application.Contracts.System;
using Mi.Domain.Exceptions;
using Mi.Domain.Helper;
using Mi.Domain.Shared.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mi.Web.Host.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;
        private readonly ILogService _logService;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }

        public void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                if (context.Exception is FriendlyException)
                {
                    context.Result = new ObjectResult(Back.Fail(context.Exception.Message));
                }
                else
                {
                    context.Result = new ObjectResult(new ResponseStructure(response_type.Error, context.Exception.Message));
                }
                _logger.LogError(context.Exception, context.Exception.Message);
                if (context.HttpContext.Items.TryGetValue("RequestId", out var temp))
                {
                    var guid = (string?)temp;
                    _logService.SetExceptionAsync(guid ?? Guid.NewGuid().ToString(), context.Exception.Message);
                }
                context.ExceptionHandled = true;
            }
        }
    }
}