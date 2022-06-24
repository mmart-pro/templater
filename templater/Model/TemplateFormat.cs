using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using templater.Classes;

namespace templater.Model;

public class TemplateFormat
{
    /// <summary>
    /// Тип формата: EnumTemplateFormats = { DOCX, XLSX }
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Тип контента для определения формата
    /// </summary>
    [MaxLength(250)]
    public string ContentType { get; set; }

    /// <summary>
    /// Имя файла с иконкой соответствующего формата
    /// </summary>
    [NotMapped]
    public string IconFileName => Id == (int)EnumTemplateFormats.XLSX
        ? "excel.svg"
        : "word.svg";
}
