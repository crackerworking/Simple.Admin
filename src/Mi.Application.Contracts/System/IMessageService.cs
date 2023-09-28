using Mi.Application.Contracts.System.Models.Result;

namespace Mi.Application.Contracts.System
{
    public interface IMessageService
    {
        Task<ResponseStructure> ReadedAsync(IList<long> msgIds);

        Task<ResponseStructure<PagingModel<SysMessageFull>>> GetMessageListAsync(MessageSearch search);

        Task<IList<HeaderMsg>> GetHeaderMsgAsync();
    }
}