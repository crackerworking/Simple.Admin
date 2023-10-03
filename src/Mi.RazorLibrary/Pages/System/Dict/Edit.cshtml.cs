using Mi.Application.Contracts.System.Models.Result;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.Dict
{
    public class EditModel : PageModel
    {
        public SysDictFull Dict { get; set; }

        public List<SysDictFull> Options { get; set; }
    }
}