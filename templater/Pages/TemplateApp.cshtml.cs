using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using templater.Model;

namespace templater.Pages;

public class TemplateAppModel : PageModel
{
    private readonly ILogger<TemplateAppModel> _logger;
    private readonly Context _context;

    /// <summary>
    /// ����������
    /// </summary>
    internal TemplateApp TemplateApp { get; private set; }

    /// <summary>
    /// �������
    /// </summary>
    internal List<Template> Templates { get; private set; }

    public TemplateAppModel(
        ILogger<TemplateAppModel> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet(string templateAppId)
    {
        TemplateApp = _context.TemplateApps.AsNoTracking().Single(app => app.Id == templateAppId);
        Templates = _context.Templates.Where(t => t.TemplateAppId == templateAppId).OrderBy(t => t.TemplateAppId).AsNoTracking().ToList();
    }
}
