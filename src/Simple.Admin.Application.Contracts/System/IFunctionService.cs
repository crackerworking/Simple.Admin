using Simple.Admin.Application.Contracts.System.Models.Function;

namespace Simple.Admin.Application.Contracts.System
{
    public interface IFunctionService
    {
        /// <summary>
        /// 新增或修改 TODO:
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        Task<MessageModel> AddOrUpdateFunctionAsync(FunctionOperation operation);

        /// <summary>
        /// 列表（带树形）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<MessageModel<IList<FunctionItem>>> GetFunctionListAsync(FunctionSearch search);

        /// <summary>
        /// 树形下拉选项
        /// </summary>
        /// <returns></returns>
        IList<TreeOption> GetFunctionTree();

        /// <summary>
        /// 单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysFunctionFull> GetAsync(long id);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> RemoveFunctionAsync(PrimaryKeys input);

        /// <summary>
        /// 所有功能（带缓存）
        /// </summary>
        /// <returns></returns>
        Task<IList<SysFunctionFull>> GetFunctionsCacheAsync();

        /// <summary>
        /// 所有功能ID
        /// </summary>
        /// <returns></returns>
        Task<IList<string>> GetAllIdsAsync();
    }
}