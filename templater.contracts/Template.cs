namespace templater.contracts;

/// <summary>
/// Шаблон документа, который требуется заполнить
/// </summary>
public class Template
{
    /// <summary>
    /// Идентификатор приложения, к которому привязан шаблон
    /// </summary>
    public string ApplicationID { get; set; }

    /// <summary>
    /// Идентификатор шаблона
    /// </summary>
    public string TemplateID { get; set; }

    /// <summary>
    /// Количество необходимых копий документа
    /// По умолчанию 1 штука
    /// </summary>
    public int Copies { get; set; } = 1;

    /// <summary>
    /// Объекты подстановок
    /// </summary>
    public Replacement[] Replacements { get; set; } = Array.Empty<Replacement>();

    /// <summary>
    /// Таблицы для заполнения
    /// </summary>
    public Table[] Tables { get; set; } = Array.Empty<Table>();

    public Template()
    {
    }

    public Template(string applicationId, string templateId, int copies = 1)
    {
        ApplicationID = applicationId;
        TemplateID = templateId;
        Copies = copies;
    }
}
