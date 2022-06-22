using System.ComponentModel.DataAnnotations;

namespace templater.Model;

/// <summary>
/// Приложение
/// </summary>
public class TemplateApp
{
    /// <summary>
    /// Идентификатор приложения
    /// </summary>
    [Key]
    [MaxLength(32)]
    public string Id { get; set; }

    /// <summary>
    /// Название приложения
    /// </summary>
    [MaxLength(80)]
    public string Name { get; set; }
}
