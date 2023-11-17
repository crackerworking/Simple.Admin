using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Simple.Admin.RazorLibrary.Pages.System.Role
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