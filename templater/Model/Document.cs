using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using templater.contracts;

namespace templater.Model;

public class Document
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(128)]
    public string FileName { get; set; }

    public OutputFormats OutputFormat { get; set; }

    public DocumentData DocumentData { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
