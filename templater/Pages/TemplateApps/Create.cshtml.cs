using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using templater.Model;

namespace templater.Pages.TemplateApps;

public class CreateModel : PageModel
{
    private readonly ILogger<CreateModel> _logger;
    private readonly Context _context;

    [BindProperty]
    [MaxLength(40)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Название приложения некорректно")]
    public string Name { get; set; }

    [BindProperty]
    [MaxLength(32)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Некорректный идентификатор приложения")]
    public string ApiRef { get; set; }

    public CreateModel(
        ILogger<CreateModel> logger,
        Context context
        )
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var app = new TemplateApp
        {
            ApiRef = ApiRef.Trim(),
            Name = Name.Trim()
        };

        // проверить уникальность apiRef
        if (await _context.TemplateApps.AnyAsync(a => a.ApiRef == app.ApiRef))
            ModelState.AddModelError(nameof(ApiRef), "Приложение с таким идентификатором уже существует");

        if (!ModelState.IsValid)
            return Page();

        _logger.LogDebug("Создание приложения {name} [{apiRef}]", app.Name, app.ApiRef);
        _context.TemplateApps.Add(app);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
