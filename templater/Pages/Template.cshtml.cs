using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using templater.Model;

namespace templater.Pages;

public class TemplateModel : PageModel
{
    private readonly ILogger<TemplateModel> _logger;
    private readonly Context _context;

    /// <summary>
    /// Шаблон
    /// </summary>
    internal Template Template { get; private set; }

    public TemplateModel(
        ILogger<TemplateModel> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet(string templateId)
    {
        Template = _context.Templates.Include(t => t.TemplateApp).AsNoTracking().Single(t => t.Id == templateId);
    }
}
