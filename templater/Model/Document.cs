using System.ComponentModel.DataAnnotations;

namespace templater.Model;

public class Document
{
    [Key]
    [MaxLength(32)]
    public string Id { get; set; }

    [MaxLength(64)]
    public string FileName { get; set; }

    public DateTime CreateTimeStamp { get; set; }
}
