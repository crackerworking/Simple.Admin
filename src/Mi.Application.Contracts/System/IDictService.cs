using Mi.Application.Contracts.System.Models.Dict;

namespace Mi.Application.Contracts.System
{
    public interface IDictService
    {
        /// <summary>
        /// 所有字典
        /// </summary>
        /// <returns></returns>
        Task<List<SysDictFull>> GetAllAsync();

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<ResponseStructure<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> AddAsync(DictPlus input);

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> UpdateAsync(DictEdit input);

        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseStructure> RemoveDictAsync(PrimaryKeys input);

        /// <summary>
        /// 获取单个字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseStructure<SysDictFull>> GetAsync(long id);

        /// <summary>
        /// 获取已有子集的字典
        /// </summary>
        /// <returns></returns>
        Task<List<Option>> GetParentListAsync();
    }
}