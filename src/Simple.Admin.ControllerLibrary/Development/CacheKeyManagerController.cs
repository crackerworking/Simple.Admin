﻿using Simple.Admin.Application.Contracts.Cache;
using Simple.Admin.Application.Contracts.Cache.Models;
using Simple.Admin.Domain.Shared.Core;
using Simple.Admin.Domain.Shared.Options;
using Simple.Admin.Domain.Shared.Response;

namespace Simple.Admin.ControllerLibrary.Development
{
    [Authorize]
    public class CacheKeyManagerController : MiControllerBase
    {
        private readonly ICacheKeyManagerService _keyService;

        public CacheKeyManagerController(ICacheKeyManagerService keyService)
        {
            _keyService = keyService;
        }

        /// <summary>
        /// 所有缓存key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Query")]
        public async Task<MessageModel<IList<Option>>> GetAllKeys([FromBody] CacheKeySearch input) => await _keyService.GetAllKeysAsync(input);

        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("Development:CacheKey:Remove")]
        public async Task<MessageModel> RemoveKey([FromBody] CacheKeyIn input) => await _keyService.RemoveKeyAsync(input);

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("Development:CacheKey:GetData")]
        public async Task<MessageModel<string>> GetData([FromBody] CacheKeyIn input) => await _keyService.GetDataAsync(input);
    }
}