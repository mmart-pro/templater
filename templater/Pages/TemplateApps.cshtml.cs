using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using templater.Model;

namespace templater.Pages;

public class TemplateAppsModel : PageModel
{
    private readonly ILogger<TemplateAppsModel> _logger;
    private readonly Context _context;

    public List<TemplateApp> TemplateApps { get; set; }

    public TemplateAppsModel(
        ILogger<TemplateAppsModel> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet(int id)
    {
        TemplateApps = _context.TemplateApps.AsNoTracking().ToList();
    }
}
