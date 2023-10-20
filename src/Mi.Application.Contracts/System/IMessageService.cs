﻿using Mi.Application.Contracts.System.Models.Message;

namespace Mi.Application.Contracts.System
{
    public interface IMessageService
    {
        /// <summary>
        /// 标记已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> ReadedAsync(PrimaryKeys input);

        /// <summary>
        /// 消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseStructure<PagingModel<SysMessageFull>>> GetMessageListAsync(MessageSearch search);

        /// <summary>
        /// 顶部导航栏未读消息
        /// </summary>
        /// <returns></returns>
        Task<IList<HeaderMsg>> GetHeaderMsgAsync();
    }
}