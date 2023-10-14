using Mi.Application.Contracts.System.Models;
using Mi.Application.Contracts.System.Models.Result;
using Mi.Domain.Shared.Options;

namespace Mi.Application.Contracts.System
{
    public interface IFunctionService
    {
        Task<ResponseStructure> AddOrUpdateFunctionAsync(FunctionOperation operation);

        Task<ResponseStructure<IList<FunctionItem>>> GetFunctionListAsync(FunctionSearch search);

        IList<TreeOption> GetFunctionTree();

        Task<SysFunctionFull> GetAsync(long id);

        Task<ResponseStructure> RemoveFunctionAsync(IList<long> ids);

        Task<IList<SysFunctionFull>> GetFunctionsCacheAsync();

        Task<IList<string>> GetAllIdsAsync();
    }
}