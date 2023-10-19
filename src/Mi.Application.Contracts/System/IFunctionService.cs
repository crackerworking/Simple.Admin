using Mi.Application.Contracts.System.Models.Function;

namespace Mi.Application.Contracts.System
{
    public interface IFunctionService
    {
        Task<ResponseStructure> AddOrUpdateFunctionAsync(FunctionOperation operation);

        Task<ResponseStructure<IList<FunctionItem>>> GetFunctionListAsync(FunctionSearch search);

        IList<TreeOption> GetFunctionTree();

        Task<SysFunctionFull> GetAsync(long id);

        Task<ResponseStructure> RemoveFunctionAsync(PrimaryKeys input);

        Task<IList<SysFunctionFull>> GetFunctionsCacheAsync();

        Task<IList<string>> GetAllIdsAsync();
    }
}