using Simple.Admin.Application.Contracts.System.Models.Dict;
using Simple.Admin.Domain.Shared.Models;
using Simple.Admin.Domain.Shared.Options;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.Application.Contracts.System
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
        Task<MessageModel<PagingModel<DictItem>>> GetDictListAsync(DictSearch search);

        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> AddAsync(DictPlus input);

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> UpdateAsync(DictEdit input);

        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MessageModel> RemoveDictAsync(PrimaryKeys input);

        /// <summary>
        /// 获取单个字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MessageModel<SysDictFull>> GetAsync(long id);

        /// <summary>
        /// 获取已有子集的字典
        /// </summary>
        /// <returns></returns>
        Task<List<Option>> GetParentListAsync();
    }
}