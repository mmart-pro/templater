namespace templater.contracts;

/// <summary>
/// Настройки выходного файла
/// </summary>
public class Output
{
    /// <summary>
    /// Имя выходного файла
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Формат выходного файла, PDF по умолчанию
    /// </summary>
    public OutputFormats Format { get; set; } = OutputFormats.PDF;

    /// <summary>
    /// Сжимать выходной файл в zip-архив, по умолчанию false,
    /// если Format = ZIP, то поле игнорируется, так как и так будет zip-архив
    /// </summary>
    public bool Zip { get; set; }
}
