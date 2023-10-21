using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Tasks;
using Mi.Domain.Shared.Core;

namespace Mi.ControllerLibrary.Development
{
    [Authorize]
    public class SysTaskController : MiControllerBase
    {
        private readonly ISysTaskService _sysTaskService;

        public SysTaskController(ISysTaskService sysTaskService)
        {
            _sysTaskService = sysTaskService;
        }

        /// <summary>
        /// 定时任务列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("SysTask:GetList")]
        public Task<MessageModel<List<TaskItem>>> GetListAsync()
        {
            return _sysTaskService.GetListAsync();
        }

        /// <summary>
        /// 更新定时任务配置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthorizeCode("SysTask:Update")]
        public Task<MessageModel> UpdateAsync([FromBody] TaskEdit input)
        {
            return _sysTaskService.UpdateAsync(input);
        }
    }
}