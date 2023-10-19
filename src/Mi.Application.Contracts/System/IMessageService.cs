using Mi.Application.Contracts.System.Models.Message;

namespace Mi.Application.Contracts.System
{
    public interface IMessageService
    {
        Task<ResponseStructure> ReadedAsync(PrimaryKeys input);

        Task<ResponseStructure<PagingModel<SysMessageFull>>> GetMessageListAsync(MessageSearch search);

        Task<IList<HeaderMsg>> GetHeaderMsgAsync();
    }
}