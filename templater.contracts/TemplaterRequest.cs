using System.Text.Json;
using System.Text.Json.Serialization;
using templater.contracts.Classes;

namespace templater.contracts;

/// <summary>
/// Запрос на формирование документов
/// </summary>
public class TemplaterRequest
{
    /// <summary>
    /// Шаблоны документов, которые требуется обработать
    /// </summary>
    public Template[] Templates { get; set; }

    /// <summary>
    /// Настройки выходного файла
    /// </summary>
    public Output Output { get; set; }

    /// <summary>
    /// Сериализация запроса в json формат
    /// </summary>
    public string ToJsonString()
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = true
        };
        jsonOptions.Converters.Add(new DateTimeConverter());
        return JsonSerializer.Serialize(this, jsonOptions);
    }

    /// <summary>
    /// Создание запроса из Json-строки
    /// </summary>
    public static TemplaterRequest FromJsonString(string json)
    {
        return JsonSerializer.Deserialize<TemplaterRequest>(json)!;
    }

    /// <summary>
    /// Конструктор с настройками выходного файла
    /// </summary>
    /// <param name="outputFormat">Формат выходного файла</param>
    /// <param name="fileName">Имя выходного файла (игнорируется для pdf)</param>
    /// <param name="zip">Сжимать выходной файл в zip-архив, по умолчанию false</param>
    public TemplaterRequest(OutputFormats outputFormat, string fileName, bool zip, params Template[] templates)
    {
        Output = new Output
        {
            Format = outputFormat,
            FileName = fileName,
            Zip = zip
        };
        Templates = templates ?? Array.Empty<Template>();
    }
    public TemplaterRequest(OutputFormats outputFormat, bool zip, params Template[] templates)
        : this(outputFormat, fileName: string.Empty, zip, templates)
    {
    }

    public TemplaterRequest(OutputFormats outputFormat, params Template[] templates)
        : this(outputFormat, fileName: string.Empty, zip: false, templates)
    {
    }

    public TemplaterRequest()
        : this(OutputFormats.PDF)
    {
    }
}
