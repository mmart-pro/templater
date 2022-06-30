using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Имя приложения для обращения через Api
    /// </summary>
    [MaxLength(32)]
    public string ApiRef { get; set; }

    /// <summary>
    /// Название приложения
    /// </summary>
    [MaxLength(40)]
    public string Name { get; set; }
}
