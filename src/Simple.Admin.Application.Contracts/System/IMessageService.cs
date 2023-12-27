using Simple.Admin.Application.Contracts.System.Models.Message;

namespace Simple.Admin.Application.Contracts.System
{
    public interface IMessageService
    {
        /// <summary>
        /// 标记已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> ReadedAsync(PrimaryKeys input);

        /// <summary>
        /// 消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<MessageModel<PagingModel<SysMessageFull>>> GetMessageListAsync(MessageSearch search);

        /// <summary>
        /// 顶部导航栏未读消息
        /// </summary>
        /// <returns></returns>
        Task<MessageModel<IList<HeaderMsg>>> GetHeaderMsgAsync();
    }
}