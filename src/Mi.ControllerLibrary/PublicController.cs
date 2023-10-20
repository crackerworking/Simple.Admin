﻿using Mi.Application.Contracts.Public;
using Mi.Domain.Shared.Core;
using Mi.Domain.Shared.Models.UI;

using Microsoft.Extensions.Caching.Memory;

namespace Mi.ControllerLibrary
{
    public class PublicController : MiControllerBase
    {
        private readonly IPublicService _publicService;
        private readonly IMemoryCache _memoryCache;

        public PublicController(IPublicService publicService, IMemoryCache memoryCache)
        {
            _publicService = publicService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <param name="guid">验证码缓存key</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<FileResult> LoginCaptcha(Guid guid)
        {
            var bytes = await _publicService.LoginCaptchaAsync(guid);
            return File(bytes, "image/png");
        }

        [HttpGet]
        [AllowAnonymous]
        public string GetLoginCaptcha(Guid guid)
        {
            return _memoryCache.Get<string>(guid.ToString())!;
        }

        /// <summary>
        /// pear-admin需要配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<PaConfigModel> Config()
        {
            return await _publicService.ReadConfigAsync();
        }
    }
}