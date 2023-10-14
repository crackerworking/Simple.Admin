using Mi.Domain.Extension;
using Mi.Domain.Shared.Models;

using UAParser;

namespace Mi.Web.Host.Middleware
{
    public class MiHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public MiHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"]; // nginx or docker
            if (ip.IsNull())
            {
                ip = context.Request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }

            var c = Parser.GetDefault().Parse(context.Request.Headers.UserAgent);
            var header = new MiHeader
            {
                Ip = ip,
                Browser = string.Concat(c.UA.Family, c.UA.Major),
                System = string.Concat(c.OS.Family, c.OS.Major)
            };

            context.Features.Set(header);

            await _next(context);
        }
    }
}