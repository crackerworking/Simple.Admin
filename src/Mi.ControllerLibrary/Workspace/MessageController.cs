using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Message;

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
        public async Task<ResponseStructure> Readed([FromBody] PrimaryKeys input) => await _messageService.ReadedAsync(input);

        [HttpPost, AuthorizeCode("Workspace:Message:Query")]
        public async Task<ResponseStructure<PagingModel<SysMessageFull>>> GetMessageList([FromBody] MessageSearch search) => await _messageService.GetMessageListAsync(search);
    }
}