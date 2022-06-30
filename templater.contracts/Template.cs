namespace templater.contracts;

/// <summary>
/// Шаблон документа, который требуется заполнить
/// </summary>
public class Template
{
    /// <summary>
    /// Идентификатор приложения, к которому привязан шаблон
    /// </summary>
    public string AppApiRef { get; set; }

    /// <summary>
    /// Идентификатор шаблона
    /// </summary>
    public string TemplateApiRef { get; set; }

    /// <summary>
    /// Количество необходимых копий документа
    /// По умолчанию 1 штука
    /// </summary>
    public int Copies { get; set; }

    /// <summary>
    /// Объекты подстановок
    /// </summary>
    public Dictionary<string, object> Replacements { get; set; }

    /// <summary>
    /// Таблицы для заполнения
    /// </summary>
    public Table[] Tables { get; set; }

    public Template(string appApiRef, string templateId, int copies, KeyValuePair<string, object>[] replacements, params Table[] tables)
    {
        AppApiRef = appApiRef;
        TemplateApiRef = templateId;
        Copies = copies;
        Replacements = new Dictionary<string, object>(replacements ?? Array.Empty<KeyValuePair<string, object>>());
        Tables = tables ?? Array.Empty<Table>();
    }

    public Template(string appApiRef, string templateId, int copies = 1)
        : this(appApiRef, templateId, copies, Array.Empty<KeyValuePair<string, object>>())
    { }

    public Template()
        : this(string.Empty, string.Empty)
    {
    }
}
