using Aspose.Cells;
using Microsoft.Extensions.Options;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using templater.contracts;
using templater.Model;

namespace templater.Classes
{
    public class Filler
    {
        private readonly Context _context;
        private readonly AppSettings _appSettings;
        private readonly ILogger<Filler> _logger;
        private readonly XlsxFiller _xlsxFiller;
        private readonly DocxFiller _docxFiller;

        /// <summary>
        /// Доступные входные форматы
        /// </summary>
        enum FilledFormat
        {
            XLSX,
            DOCX,
            PDF
        }

        /// <summary>
        /// Сформированные документы
        /// </summary>
        record FilledDoc(byte[] Data, FilledFormat InputFormat, int Copies, string TemplateId);

        /// <summary>
        /// Конструктор
        /// </summary>
        public Filler(
            Context context,
            IOptions<AppSettings> appSettings,
            ILogger<Filler> logger,
            XlsxFiller xlsxFiller,
            DocxFiller docxFiller
            )
        {
            _context = context;
            _appSettings = appSettings.Value;
            _logger = logger;
            _xlsxFiller = xlsxFiller;
            _docxFiller = docxFiller;
        }

        public byte[] Fill(TemplaterRequest contract)
        {
            if (contract.Templates.Length < 1)
                throw new ArgumentException("Запрос не содержит шаблонов");

            var convertToPdf = contract.Output.Format == OutputFormats.PDF;

            var filledDocs = new List<FilledDoc>();
            // заполнить каждый из шаблонов
            foreach (var template in contract.Templates)
            {

                // загрузить файл
                var srcPath = _appSettings.TEMPLATE_PATH + template.ApplicationID + Path.DirectorySeparatorChar + template.TemplateID;
                _logger.LogDebug("Загрузка файла шаблона {srcPath}", srcPath);
                var data = File.ReadAllBytes(srcPath);

                // для статистики сохраним в БД, что шаблон использовался
                _logger.LogDebug("Обновление статистики использования шаблона {srcPath}", srcPath);
                var dbTemplate = _context.Templates.SingleOrDefault(t => t.TemplateAppId == template.ApplicationID && t.Id == template.TemplateID);
                dbTemplate!.LastUsedDateTime = DateTime.Now;
                _context.SaveChanges();

                _logger.LogDebug("Заполнение шаблона {srcPath}", srcPath);

                if (template.TemplateID.EndsWith("xlsx", StringComparison.OrdinalIgnoreCase))
                    filledDocs.Add(new(_xlsxFiller.Fill(data, template, convertToPdf), convertToPdf ? FilledFormat.PDF : FilledFormat.XLSX, template.Copies, template.TemplateID));
                else if (template.TemplateID.EndsWith("docx", StringComparison.OrdinalIgnoreCase))
                    filledDocs.Add(new(_docxFiller.Fill(data, template, convertToPdf), convertToPdf ? FilledFormat.PDF : FilledFormat.DOCX, template.Copies, template.TemplateID));
                else
                    throw new Exception("Неподдерживаемый формат шаблона");
            }

            // слияние документов в общий файл
            byte[] result = Array.Empty<byte>();

            // если одна копия, то ничего не сливаем
            if (filledDocs.Count == 1 && filledDocs[0].Copies == 1)
                result = filledDocs[0].Data;
            else
            {
                if (contract.Output.Format == OutputFormats.PDF)
                {
                    _logger.LogDebug("Слияние документов в выходной PDF-файл...");
                    result = MergePDF(filledDocs);
                }
                else if (contract.Output.Format == OutputFormats.XLSX)
                {
                    // защита от дурака
                    if (filledDocs.Any(d => d.InputFormat != FilledFormat.XLSX))
                        throw new Exception("Формат входных документов не соответствует XLSX");
                    _logger.LogDebug("Слияние документов в выходной XLSX-файл...");
                    result = MergeXLSX(filledDocs);
                }
                else if (contract.Output.Format == OutputFormats.DOCX)
                {
                    // защита от дурака
                    if (filledDocs.Any(d => d.InputFormat != FilledFormat.DOCX))
                        throw new Exception("Формат входных документов не соответствует DOCX");
                    _logger.LogDebug("Слияние документов в выходной DOCX-файл...");
                    result = MergeDOCX(filledDocs);
                }
                else if (contract.Output.Format != OutputFormats.ZIP)
                    throw new Exception("Формат входных документов не поддреживается");
            }

            // если требуется сжатие
            if (contract.Output.Format == OutputFormats.ZIP)
            {
                _logger.LogDebug("Слияние документов в выходной ZIP-файл...");
                result = CompressToZip(filledDocs);
            }

            return result;
        }

        /// <summary>
        /// Создание zip-архива с документами
        /// </summary>
        /// <returns></returns>
        static byte[] CompressToZip(IEnumerable<FilledDoc> filledDocs)
        {
#warning не реализовано сжатие в zip
            throw new Exception("Не реализовано сжатие в zip");
        }

        /// <summary>
        /// Слияние DOCX
        /// </summary>
        static byte[] MergeDOCX(IEnumerable<FilledDoc> filledDocs)
        {
#warning не реализован merge DOCX-документов
            throw new Exception("Не реализован merge DOCX-документов");
            //using var outStream = new MemoryStream();
            //using (var output = new Workbook())
            //{
            //    var i = 1;
            //    foreach (var xls in filledDocs)
            //    {
            //        using var srcStream = new MemoryStream(xls.Data);
            //        using var src = new Workbook(srcStream);
            //        for (var c = 0; i < xls.Copies; c++)
            //        {
            //            // делаем новую страницу
            //            var newSheet = output.Worksheets.Add($"Лист {i} - {xls.TemplateId}");
            //            // и копируем в другой файл
            //            newSheet.Copy(src.Worksheets[0]);
            //            i++;
            //        }
            //    }
            //    // удаление пустого диста
            //    output.Worksheets.RemoveAt(0);
            //    output.Save(outStream, SaveFormat.Xlsx);
            //}
            //return outStream.ToArray();
        }

        /// <summary>
        /// Слияние XLS
        /// </summary>
        static byte[] MergeXLSX(IEnumerable<FilledDoc> filledDocs)
        {
            using var outStream = new MemoryStream();
            using (var output = new Workbook())
            {
                var i = 1;
                foreach (var xls in filledDocs)
                {
                    using var srcStream = new MemoryStream(xls.Data);
                    using var src = new Workbook(srcStream);
                    for (var c = 0; i < xls.Copies; c++)
                    {
                        // делаем новую страницу
                        var newSheet = output.Worksheets.Add($"Лист {i} - {xls.TemplateId}");
                        // и копируем в другой файл
                        newSheet.Copy(src.Worksheets[0]);
                        i++;
                    }
                }
                // удаление пустого диста
                output.Worksheets.RemoveAt(0);
                output.Save(outStream, SaveFormat.Xlsx);
            }
            return outStream.ToArray();
        }

        /// <summary>
        /// Слияние PDF с нужным количеством копий документа
        /// </summary>
        static byte[] MergePDF(IEnumerable<FilledDoc> filledDocs)
        {
            using var outStream = new MemoryStream();
            using (var output = new PdfDocument())
            {
                foreach (var doc in filledDocs)
                {
                    using var srcStream = new MemoryStream(doc.Data);
                    using var tmp = PdfReader.Open(srcStream, PdfDocumentOpenMode.Import);
                    for (var i = 0; i < doc.Copies; i++)
                        foreach (var p in tmp.Pages)
                            output.AddPage(p);
                }
                output.Save(outStream);
            }
            return outStream.ToArray();
        }
    }
}
