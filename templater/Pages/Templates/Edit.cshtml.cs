using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using templater.Classes;
using templater.Model;

namespace templater.Pages.Templates;

public class EditModel : PageModel
{
    private readonly Context _context;

    /// <summary>
    /// Приложение
    /// </summary>
    public TemplateApp TemplateApp { get; set; }

    /// <summary>
    /// Шаблон
    /// </summary>
    public Template Template { get; set; }

    /// <summary>
    /// Идентификатор шаблона
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public int TemplateId { get; set; }

    /// <summary>
    /// Идентификатор шаблона для Api из формы (post)
    /// </summary>
    [BindProperty]
    [MaxLength(32)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Некорректный идентификатор шаблона")]
    public string ApiRef { get; set; }

    /// <summary>
    /// Файл шаблона
    /// </summary>
    [BindProperty]
    public List<IFormFile> TemplateFile { get; set; }

    public EditModel(
        Context context
        )
    {
        _context = context;
    }

    private void Init(int templateId)
    {
        Template = _context.Templates
            .AsNoTracking().SingleOrDefault(t => t.Id == templateId);
        if (Template == null)
            return;
        TemplateApp = _context.TemplateApps.AsNoTracking().Single(a => a.Id == Template.TemplateAppId);
    }

    public IActionResult OnGet()
    {
        Init(TemplateId);
        if (Template == null)
            return NotFound();

        ApiRef = Template.ApiRef;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Init(TemplateId);
        if (Template == null)
            return NotFound();

        if (!ModelState.IsValid)
            return Page();

        Template.ApiRef = ApiRef.Trim();
        if (await _context.Templates.AnyAsync(t => t.ApiRef == Template.ApiRef && t.TemplateAppId == Template.TemplateAppId && t.Id != Template.Id))
        {
            ModelState.AddModelError(nameof(ApiRef), "Шаблон с таким идентификатором уже существует");
            return Page();
        }

        if (TemplateFile.Count > 0)
        {
            var inputFile = TemplateFile[0];
            if (inputFile.Length == 0)
            {
                ModelState.AddModelError(nameof(TemplateFile), "Файл шаблона имеет нулевой размер");
                return Page();
            }

            // определить формат
            var templateFormat = await _context.TemplateFormats.SingleOrDefaultAsync(f => f.ContentType == inputFile.ContentType);
            if (templateFormat == null)
            {
                ModelState.AddModelError(nameof(TemplateFile), "Формат шаблона не поддерживается");
                return Page();
            }
            Template.TemplateFormatId = templateFormat.Id;

            using var m = new MemoryStream();
            await inputFile.CopyToAsync(m);

            var templateData = _context.TemplateDatas.Single(t => t.Id == Template.Id);
            templateData.Data = m.ToArray();
            Template.DataSize = templateData.Data.Length;
        }

        _context.Attach(Template).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return RedirectToPage("/TemplateApps/Edit", new { Template.TemplateAppId });
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var template = await _context.Templates.FindAsync(TemplateId);
        if (template == null)
            return NotFound();
        _context.Templates.Remove(template);
        await _context.SaveChangesAsync();

        return RedirectToPage("/TemplateApps/Edit", new { template.TemplateAppId });
    }

    public async Task<IActionResult> OnPostDownloadAsync()
    {
        var template = await _context.Templates
            .Include(t => t.TemplateData)
            .SingleOrDefaultAsync(t => t.Id == TemplateId);
        if (template == null)
            return NotFound();

        var fileName = template.ApiRef;
        var ext = Enum.GetName(typeof(EnumTemplateFormats), template.TemplateFormatId);
        if (!string.IsNullOrEmpty(ext) && !fileName.ToLower().EndsWith("." + ext.ToLower()))
            fileName += "." + ext.ToLower();

        return File(template.TemplateData.Data, "application/octet-stream", fileName);
    }
}
