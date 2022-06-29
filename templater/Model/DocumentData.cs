using System.ComponentModel.DataAnnotations;

namespace templater.Model;

public class DocumentData
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Данные документа
    /// </summary>
    public byte[] Data { get; set; }
}
