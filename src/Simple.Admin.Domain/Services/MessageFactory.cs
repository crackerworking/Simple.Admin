using Simple.Admin.Domain.Exceptions;

using Microsoft.Extensions.DependencyInjection;

using Simple.Admin.Domain.DataAccess;
using Simple.Admin.Domain.Entities.System;
using Simple.Admin.Domain.Extension;

using Simple.Admin.Domain.Shared;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.Domain.Services
{
    public class MessageFactory : IMessageFactory
    {
        private static Lazy<MessageFactory> _lazy => new Lazy<MessageFactory>(() => new MessageFactory());

        public static MessageFactory Instance => _lazy.Value;

        /// <summary>
        /// 系统消息写入
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="users">接收用户ID</param>
        /// <returns></returns>
        public void WriteMessage(string title, string content, IEnumerable<long> users)
        {
            using var p = App.Provider.CreateScope();
            var messageRepo = p.ServiceProvider.GetRequiredService<IRepository<SysMessage>>();
            var _miUser = p.ServiceProvider.GetRequiredService<ICurrentUser>();
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content)) throw new FriendlyException("消息的标题和内容不能为空");
            if (users.IsNull()) throw new FriendlyException("接收消息用户不能为空");

            var now = DateTime.Now;
            var list = users.Select(x => new SysMessage { Title = title, Content = content, Readed = 0, CreatedBy = _miUser.UserId, CreatedOn = now, ReceiveUser = x }).ToList();
            messageRepo.AddRangeAsync(list).ConfigureAwait(false);
        }
    }
}