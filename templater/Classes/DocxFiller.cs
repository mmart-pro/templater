using templater.contracts;

namespace templater.Classes;

public class DocxFiller
{
    private readonly ILogger<DocxFiller> _logger;

    public byte[] Fill(byte[] template, Template contract, bool convertToPdf)
    {
#warning не реализовано заполнение docx документов
        return null;
    }

    public DocxFiller(
        ILogger<DocxFiller> logger
        )
    {
        _logger = logger;
    }
}
