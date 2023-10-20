using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Dict;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.System
{
    [Authorize]
    public class DictController : MiControllerBase
    {
        private readonly IDictService _dictService;

        public DictController(IDictService dictService)
        {
            _dictService = dictService;
        }

        /// <summary>
        /// 字典列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<ResponseStructure<PagingModel<DictItem>>> GetDictList([FromBody] DictSearch search)
            => await _dictService.GetDictListAsync(search);

        /// <summary>
        /// 新增字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Dict:Add")]
        public async Task<ResponseStructure> AddAsync([FromBody] DictPlus operation)
            => await _dictService.AddAsync(operation);

        /// <summary>
        /// 更新字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Dict:Update")]
        public async Task<ResponseStructure> UpdateAsync([FromBody] DictEdit operation)
            => await _dictService.UpdateAsync(operation);

        /// <summary>
        /// 移除字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Dict:Remove")]
        public async Task<ResponseStructure> RemoveDict([FromBody] PrimaryKeys input)
            => await _dictService.RemoveDictAsync(input);

        /// <summary>
        /// 获取已有子集的字典
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Dict:Query")]
        public async Task<List<Option>> GetParentList()
            => await _dictService.GetParentListAsync();
    }
}