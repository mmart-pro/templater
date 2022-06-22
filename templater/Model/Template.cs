using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace templater.Model;

public class Template
{
    [MaxLength(32)]
    public string TemplateAppId { get; set; }

    public TemplateApp TemplateApp { get; set; }

    [MaxLength(32)]
    public string Id { get; set; }

    public DateTime? CreateTimeStamp { get; set; }

    public DateTime? TimeStamp { get; set; }

    public DateTime? LastUsedDateTime { get; set; }

    [NotMapped]
    public string ImageName =>
        Id.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase)
        ? "excel"
        : Id.EndsWith("docx", StringComparison.OrdinalIgnoreCase)
        ? "word"
        : "document-exclamation";
}
