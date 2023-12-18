using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Message;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.ControllerLibrary.Workspace
{
    [Authorize]
    public class MessageController : MiControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// 标记已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("Workspace:Message:Readed")]
        public async Task<MessageModel> Readed([FromBody] PrimaryKeys input) => await _messageService.ReadedAsync(input);

        /// <summary>
        /// 消息列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("Workspace:Message:Query")]
        public async Task<MessageModel<PagingModel<SysMessageFull>>> GetMessageList([FromBody] MessageSearch search) => await _messageService.GetMessageListAsync(search);
    }
}