using Mi.Application.Contracts.System;
using Mi.Application.Contracts.System.Models.Result;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.Dict
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
            Options = _dictService.GetAll();
        }
    }
}