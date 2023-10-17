using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Tasks;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.Development.SysTask
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