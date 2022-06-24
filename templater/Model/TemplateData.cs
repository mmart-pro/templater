using System.ComponentModel.DataAnnotations;

namespace templater.Model;

public class TemplateData
{
    /// <summary>
    /// Уникальный Id в БД для роутов
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Данные шаблона
    /// </summary>
    public byte[] Data { get; set; }
}
