using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models;

namespace Mi.ControllerLibrary.Workspace
{
    [ApiRoute]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost, AuthorizeCode("Workspace:Message:Readed")]
        public async Task<ResponseStructure> Readed(IList<long> ids) => await _messageService.ReadedAsync(ids);

        [HttpPost, AuthorizeCode("Workspace:Message:Query")]
        public async Task<ResponseStructure<PagingModel<SysMessageFull>>> GetMessageList(MessageSearch search) => await _messageService.GetMessageListAsync(search);
    }
}