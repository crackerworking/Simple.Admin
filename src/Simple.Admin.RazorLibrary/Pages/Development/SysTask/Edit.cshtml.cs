using Microsoft.AspNetCore.Mvc.RazorPages;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Tasks;

namespace Simple.Admin.RazorLibrary.Pages.Development.SysTask
{
    public class EditModel : PageModel
    {
        private readonly ISysTaskService _sysTaskService;

        public TaskItem TaskItem { get; set; }

        public EditModel(ISysTaskService sysTaskService)
        {
            _sysTaskService = sysTaskService;
        }

        public async Task OnGetAsync(long id)
        {
            TaskItem = await _sysTaskService.GetAsync(id);
        }
    }
}