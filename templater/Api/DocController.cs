using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using templater.Classes;
using templater.contracts;
using templater.Model;

namespace docs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocController : ControllerBase
{
    private readonly Context _context;
    private readonly ILogger<DocController> _logger;
    private readonly Filler _filler;

    public DocController(
        Context context,
        ILogger<DocController> logger,
        Filler filler
    )
    {
        _context = context;
        _logger = logger;
        _filler = filler;
    }

    /// <summary>
    /// Получение документа по публичному идентификатору
    /// </summary>
    /// <param name="id">Публичный идентификатор документа</param>
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] string id)
    {
        try
        {
            var publicId = PublicId.Decode(id);
            var doc = _context.Documents
                .Include(d => d.DocumentData)
                .Single(d => d.Id == publicId);

            var ext = Path.GetExtension(doc.FileName).ToUpperInvariant();
            if (doc.OutputFormat == OutputFormats.PDF)
                return File(doc.DocumentData.Data, "application/pdf");
            else
                return File(doc.DocumentData.Data, "application/octet-stream", doc.FileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении документа");
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    /// <summary>
    /// Генерация документа
    /// </summary>
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public IActionResult Generate() //[FromBody] TemplaterRequest templaterRequest)
    {
#warning какой-то маразм из-за чего не цепляется ФромБоди не ясно...
        StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
        var s = reader.ReadToEndAsync();
        s.Wait();
        var templaterRequest = JsonSerializer.Deserialize<TemplaterRequest>(s.Result); // new TemplaterRequest();

#warning по-хорошему надо закрывать на авторизацию
        try
        {
            var templates = templaterRequest.Templates.Aggregate(string.Empty, (acc, c) => acc + $" {c.AppApiRef}/{c.TemplateApiRef}").Trim();
            _logger.LogDebug("Получен запрос на формирование документов OutputFormat={format}, Шаблоны: [{templates}]",
                templaterRequest.Output.Format, templates);

            // получить документы
            var data = _filler.Fill(templaterRequest);

            var fileName = templaterRequest.Output.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = templaterRequest.Templates.First().TemplateApiRef +
                    DateTime.Now.ToString("_ddMMyy_HHmmss") + "." + templaterRequest.Output.Format.ToString().ToLower();

            var doc = new Document
            {
                FileName = fileName,
                OutputFormat = templaterRequest.Output.Format,
                DocumentData = new DocumentData { Data = data },
                CreateTimeStamp = DateTime.Now
            };
            _context.Documents.Add(doc);
            _context.SaveChanges();

            var publicId = PublicId.Encode(doc.Id);
            _logger.LogDebug("Документ ID={id}/PublicID={publicId} готов", doc.Id, publicId);

            return Ok(publicId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при формировании документа");
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }
}
