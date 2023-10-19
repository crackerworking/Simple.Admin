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

        [HttpPost]
        public Task<ResponseStructure<List<TaskItem>>> GetListAsync()
        {
            return _sysTaskService.GetListAsync();
        }

        [HttpPost]
        public Task<ResponseStructure> UpdateAsync([FromBody] TaskEdit input)
        {
            return _sysTaskService.UpdateAsync(input);
        }
    }
}