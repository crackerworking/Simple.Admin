using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models;

namespace Mi.ControllerLibrary.System
{
    [ApiRoute]
    [Authorize]
    public class SysLogController : ControllerBase
    {
        private readonly ILogService _logService;

        public SysLogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpPost, AuthorizeCode("System:LoginLog:Query")]
        public async Task<ResponseStructure<PagingModel<SysLoginLogFull>>> GetLoginLogList([FromBody] LoginLogSearch search)
        {
            return await _logService.GetLoginLogListAsync(search);
        }

        [HttpPost, AuthorizeCode("System:ActionLog:Query")]
        public async Task<ResponseStructure<PagingModel<SysLogFull>>> GetLogList([FromBody] LogSearch search) => await _logService.GetLogListAsync(search);
    }
}