using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Function;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Models;
using Simple.Admin.Domain.Shared.Options;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.ControllerLibrary.System
{
    [Authorize]
    public class FunctionController : MiControllerBase
    {
        private readonly IFunctionService _functionService;

        public FunctionController(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        /// <summary>
        /// 列表（带树形）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Query")]
        public async Task<MessageModel> GetFunctionList([FromBody] FunctionSearch search)
        {
            return await _functionService.GetFunctionListAsync(search);
        }

        /// <summary>
        /// 新增或修改 TODO:
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:AddOrUpdate")]
        public async Task<MessageModel> AddOrUpdateFunction([FromBody] FunctionOperation operation)
            => await _functionService.AddOrUpdateFunctionAsync(operation);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Remove")]
        public async Task<MessageModel> RemoveFunction([FromBody] PrimaryKeys input)
            => await _functionService.RemoveFunctionAsync(input);

        /// <summary>
        /// 树形下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Query")]
        public IList<TreeOption> GetFunctionTree() => _functionService.GetFunctionTree();
    }
}