using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using templater.Model;

namespace templater.Pages.Templates;

public class CreateModel : PageModel
{
    private readonly Context _context;

    /// <summary>
    /// ���������� �������
    /// </summary>
    public TemplateApp TemplateApp { get; set; }

    /// <summary>
    /// ������������� ���������� �� ����� (post)
    /// </summary>
    [BindProperty(SupportsGet = true)]
    public int TemplateAppId { get; set; }

    /// <summary>
    /// ������������� ������� ��� Api
    /// </summary>
    [BindProperty]
    [MaxLength(32)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "������������ ������������� �������")]
    public string ApiRef { get; set; }

    /// <summary>
    /// ���� �������
    /// </summary>
    [BindProperty]
    [MinLength(1, ErrorMessage = "�������� ����")]
    public List<IFormFile> TemplateFile { get; set; }

    public CreateModel(
        Context context
        )
    {
        _context = context;
    }

    void Init(int appId)
    {
        TemplateApp = _context.TemplateApps
            .AsNoTracking().SingleOrDefault(a => a.Id == appId);
    }

    public IActionResult OnGet()
    {
        Init(TemplateAppId);
        if (TemplateApp == null)
            return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Init(TemplateAppId);
        if (TemplateApp == null)
            return NotFound();

        if (!ModelState.IsValid)
            return Page();

        var inputFile = TemplateFile[0];
        if (inputFile.Length == 0)
        {
            ModelState.AddModelError(nameof(TemplateFile), "���� ������� ����� ������� ������");
            return Page();
        }

        // ���������� ������
        var templateFormat = await _context.TemplateFormats.SingleOrDefaultAsync(f => f.ContentType == inputFile.ContentType);
        if (templateFormat == null)
        {
            ModelState.AddModelError(nameof(TemplateFile), "������ ������� �� ��������������");
            return Page();
        }

        var template = new Template
        {
            TemplateAppId = TemplateAppId,
            ApiRef = ApiRef.Trim(),
            TemplateFormatId = templateFormat.Id
        };

        if (await _context.Templates.AnyAsync(t => t.ApiRef == template.ApiRef && t.TemplateAppId == TemplateAppId))
        {
            ModelState.AddModelError(nameof(ApiRef), "������ � ����� ��������������� ��� ����������");
            return Page();
        }

        using (var m = new MemoryStream())
        {
            await inputFile.CopyToAsync(m);
            template.TemplateData = new TemplateData { Data = m.ToArray() };
            template.DataSize = template.TemplateData.Data.Length;
        }

        _context.Templates.Add(template);
        await _context.SaveChangesAsync();

        return RedirectToPage("/TemplateApps/Edit", new { TemplateAppId });
    }
}
