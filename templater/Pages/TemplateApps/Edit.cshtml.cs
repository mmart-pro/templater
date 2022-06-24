using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using templater.Model;

namespace templater.Pages.TemplateApps;

public class EditModel : PageModel
{
    private readonly Context _context;

    [Required]
    [BindProperty(SupportsGet = true)]
    public int TemplateAppId { get; set; }

    [BindProperty]
    [MaxLength(40)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Название приложения некорректно")]
    public string Name { get; set; }

    [BindProperty]
    [MaxLength(16)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Некорректный идентификатор приложения")]
    public string ApiRef { get; set; }

    /// <summary>
    /// Приложение
    /// </summary>
    [BindProperty]
    public TemplateApp TemplateApp { get; set; }

    /// <summary>
    /// Шаблоны
    /// </summary>
    public List<Template> Templates { get; set; }

    public EditModel(
        Context context
        )
    {
        _context = context;
    }

    void Init(int appId)
    {
        TemplateApp = _context.TemplateApps
            .AsNoTracking().SingleOrDefault(a => a.Id == appId);
        Templates = _context.Templates
            .Include(t => t.TemplateFormat)
            .Where(t => t.TemplateAppId == appId)
            .OrderBy(t => t.ApiRef)
            .AsNoTracking().ToList();
    }

    public IActionResult OnGet()
    {
        Init(TemplateAppId);
        if (TemplateApp == null)
            return NotFound();

        ApiRef = TemplateApp.ApiRef;
        Name = TemplateApp.Name;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Init(TemplateAppId);
        if (TemplateApp == null)
            return NotFound();

        if (!ModelState.IsValid)
            return Page();

        TemplateApp.ApiRef = ApiRef.Trim();
        TemplateApp.Name = Name.Trim();

        // проверить уникальность id
        if (await _context.TemplateApps.AnyAsync(a => a.ApiRef == TemplateApp.ApiRef && a.Id != TemplateAppId))
            ModelState.AddModelError(nameof(ApiRef), "Приложение с таким идентификатором уже существует");

        if (!ModelState.IsValid)
            return Page();

        _context.Attach(TemplateApp).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var app = await _context.TemplateApps.SingleOrDefaultAsync(a => a.Id == TemplateAppId);
        if (app != null)
        {
            _context.TemplateApps.Remove(app);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage("Index");
    }
}
