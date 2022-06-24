using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace templater.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
#warning наверняка можно как-то более изящно сделать index page
            return RedirectToPage("/TemplateApps/Index");
        }
    }
}