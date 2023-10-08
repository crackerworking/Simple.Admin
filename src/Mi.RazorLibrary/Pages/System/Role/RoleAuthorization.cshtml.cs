using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mi.RazorLibrary.Pages.System.Role
{
    public class RoleAuthorizationModel : PageModel
    {
        public long Id { get; set; }

        public async Task OnGetAsync(long? id)
        {
            Id = id.GetValueOrDefault();
        }
    }
}