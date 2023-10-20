using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Log;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.System
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
        [HttpPost, AuthorizeCode("System:LoginLog:Query")]
        public async Task<ResponseStructure<PagingModel<SysLoginLogFull>>> GetLoginLogList([FromBody] LoginLogSearch search)
        {
            return await _logService.GetLoginLogListAsync(search);
        }

        /// <summary>
        /// 操作日志列表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost, AuthorizeCode("System:ActionLog:Query")]
        public async Task<ResponseStructure<PagingModel<SysLogFull>>> GetLogList([FromBody] LogSearch search) => await _logService.GetLogListAsync(search);
    }
}