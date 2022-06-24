using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using templater.Model;

namespace templater.Pages.TemplateApps;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Context _context;

    public List<TemplateApp> TemplateApps { get; set; }

    public IndexModel(
        ILogger<IndexModel> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
        _logger.LogDebug("Загрузка приложений...");
        TemplateApps = _context.TemplateApps.OrderBy(a=>a.Name).AsNoTracking().ToList();
    }
}
