using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoDb.Web.Pages
{
    public class IndexModel : PageModel
    {
        public ActionResult OnGetAsync()
        {
            return Page();
        }
    }
}