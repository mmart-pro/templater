using System.Text.Json;

namespace templater.contracts;

/// <summary>
/// Запрос на формирование документов
/// </summary>
public class TemplaterRequest
{
    /// <summary>
    /// Шаблоны документов, которые требуется обработать
    /// </summary>
    public Template[] Templates { get; set; } = Array.Empty<Template>();

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
            WriteIndented = true
        };
        return JsonSerializer.Serialize(this, jsonOptions);
    }

    /// <summary>
    /// Создание запроса из Json-строки
    /// </summary>
    public static TemplaterRequest FromJsonString(string json)
    {
        return JsonSerializer.Deserialize<TemplaterRequest>(json)!;
    }

    public TemplaterRequest()
    {
        Output = new Output();
    }

    /// <summary>
    /// Конструктор с настройками выходного файла
    /// </summary>
    /// <param name="outputFormat">Формат выходного файла</param>
    /// <param name="zip">Сжимать выходной файл в zip-архив, по умолчанию false</param>
    public TemplaterRequest(OutputFormats outputFormat, bool zip = false)
        :this()
    {
        Output.Format = outputFormat;
        Output.Zip = zip;
    }
}
