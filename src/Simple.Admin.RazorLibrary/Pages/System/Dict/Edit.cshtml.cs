using Microsoft.AspNetCore.Mvc.RazorPages;

using Simple.Admin.Application.Contracts.System;
using Simple.Admin.Application.Contracts.System.Models.Dict;

namespace Simple.Admin.RazorLibrary.Pages.System.Dict
{
    public class EditModel : PageModel
    {
        private readonly IDictService _dictService;

        public SysDictFull Dict { get; set; } = new();

        public List<SysDictFull> Options { get; set; } = new();

        public EditModel(IDictService dictService)
        {
            _dictService = dictService;
        }

        public async Task OnGetAsync(long? id)
        {
            var r1 = await _dictService.GetAsync(id.GetValueOrDefault());
            if (r1.IsOk())
            {
                Dict = r1.Result!;
            }
            Options = await _dictService.GetAllAsync();
        }
    }
}