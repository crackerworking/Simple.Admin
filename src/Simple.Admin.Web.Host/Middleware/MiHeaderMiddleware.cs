using System.Net;

using Simple.Admin.Domain.Extension;
using Simple.Admin.Domain.Shared.Models;

using UAParser;

namespace Simple.Admin.Web.Host.Middleware
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
                ip = context.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }

            if (!ip.IsNull() && Array.IndexOf(new IPAddress[] { IPAddress.Any, IPAddress.None, IPAddress.Broadcast, IPAddress.Loopback, new IPAddress(new byte[] { 0, 0, 0, 1 }) }, IPAddress.Parse(ip!)) == -1)
            {
                ;
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