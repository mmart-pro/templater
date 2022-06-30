using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace templater.Model;

public class Template
{
    /// <summary>
    /// Уникальный Id в БД для роутов
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор приложения
    /// </summary>
    [MaxLength(32)]
    public int TemplateAppId { get; set; }

    public TemplateApp TemplateApp { get; set; }

    /// <summary>
    /// Имя шаблона для обращения через Api
    /// должно быть уникальным в пределах приложения
    /// по сути это имя файла
    /// </summary>
    [MaxLength(32)]
    public string ApiRef { get; set; }

    /// <summary>
    /// Данные шаблона
    /// </summary>
    public TemplateData TemplateData { get; set; }

    /// <summary>
    /// Размер данных
    /// </summary>
    public long DataSize { get; set; }

    /// <summary>
    /// Идентификатор формата
    /// </summary>
    public int TemplateFormatId { get; set; }

    /// <summary>
    /// Формат файла и его content-type
    /// </summary>
    public TemplateFormat TemplateFormat { get; set; }

    public DateTime CreateTimeStamp { get; set; }

    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Дата и время последнего использования
    /// </summary>
    public DateTime? LastUsedDateTime { get; set; }
}
