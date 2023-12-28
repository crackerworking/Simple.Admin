using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Function;
using Simple.Admin.Domain.Shared.Core;

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
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Query")]
        public async Task<MessageModel> GetFunctionTree()
        {
            return await _functionService.GetFunctionTreeAsync();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        [HttpPost,AuthorizeCode("System:Function:Add")]
        public Task<MessageModel> AddFunction([FromBody] FunctionOperation operation) => _functionService.AddFunctionAsync(operation);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Update")]
        public Task<MessageModel> UpdateFunction([FromBody] FunctionOperation operation) => _functionService.UpdateFunctionAsync(operation);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Remove")]
        public async Task<MessageModel> RemoveFunction([FromBody] PrimaryKeys input)
            => await _functionService.RemoveFunctionAsync(input);

        /// <summary>
        /// 功能列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("System:Function:Query")]
        public Task<MessageModel<IList<SysFunctionFull>>> GetFunctionList([FromBody] FunctionDto dto) => _functionService.GetFunctionList(dto);
    }
}