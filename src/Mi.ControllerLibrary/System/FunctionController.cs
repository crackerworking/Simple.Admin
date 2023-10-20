﻿using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Function;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.System
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
        public async Task<ResponseStructure> GetFunctionList([FromBody] FunctionSearch search)
        {
            return await _functionService.GetFunctionListAsync(search);
        }

        /// <summary>
        /// 新增或修改 TODO:
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:AddOrUpdate")]
        public async Task<ResponseStructure> AddOrUpdateFunction([FromBody] FunctionOperation operation)
            => await _functionService.AddOrUpdateFunctionAsync(operation);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Remove")]
        public async Task<ResponseStructure> RemoveFunction([FromBody] PrimaryKeys input)
            => await _functionService.RemoveFunctionAsync(input);

        /// <summary>
        /// 树形下拉选项
        /// </summary>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:Function:Query")]
        public IList<TreeOption> GetFunctionTree() => _functionService.GetFunctionTree();
    }
}