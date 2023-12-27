using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Log;
using Simple.Admin.Domain.Shared.Core;

namespace Simple.Admin.ControllerLibrary.System
{
    [Authorize]
    public class SysLogController : MiControllerBase
    {
        private readonly ILogService _logService;

        public SysLogController(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// 登录日志列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [AuthorizeCode("System:LoginLog")]
        public async Task<MessageModel<PagingModel<SysLoginLogFull>>> GetLoginLogList([FromBody] LoginLogSearch search)
        {
            return await _logService.GetLoginLogListAsync(search);
        }
    }
}