using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Mat.Web.Pages
{
    public class IndexModel : PageModel
    {
        public ActionResult OnGetAsync()
        {
            return Page();
        }
    }
}