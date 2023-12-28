using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

using Simple.Admin.Domain.DataAccess;
using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Options;

namespace Simple.Admin.Domain.Hubs
{
    /// <summary>
    /// 消息中心，自动发送通知； 禁止从中心以外使用。
    /// </summary>
    public class NoticeHub : Hub
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public async Task SendMessage(string userName)
        {
            using var p = App.Provider.CreateScope();
            var dapper = p.ServiceProvider.GetRequiredService<IDapperRepository>();
            var msg = await dapper.QueryFirstOrDefaultAsync<Option>(@$"select m.Title as Name,m.Content as Value from SysMessage m
                            inner join SysUser u on m.ReceiveUser=u.Id
                            where m.IsDeleted=0 and m.Readed=0 and lower(u.UserName)='{userName?.ToLower()}' order by m.CreatedOn asc limit 1;");
            var title = string.Empty;
            var content = string.Empty;
            if (msg != null)
            {
                title = msg.Name!;
                content = msg.Value!;
            }
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) return;
            await Clients.All.SendAsync("receiveMessage", title, content);
        }
    }
}