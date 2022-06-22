using Aspose.Cells;
using docs.Classes;
using templater.contracts;

namespace templater.Classes;

public class XlsxFiller
{
    private readonly ILogger<XlsxFiller> _logger;

    public byte[] Fill(byte[] template, Template contract, bool convertToPdf)
    {
        using var input = new MemoryStream(template);

        var workbook = new Workbook(input);
        var cells = workbook.Worksheets[0].Cells;

        // заполнить подстановки
        if (contract.Replacements.Length > 0)
        {
            _logger.LogDebug("Заполнение подстановок в документе...");
            FillXlsHeaders(cells, contract.Replacements);
        }

        // заполнить таблицы
        if (contract.Tables.Length > 0)
        {
            var minRow = cells.MinDataRow;
            foreach (var table in contract.Tables)
            {
                var rowId = table.RowId;    // rowNum по дефолту
                // найти таблицу
                var templateRowIndex = GetXlsRowIndex(cells, rowId, minRow);
                // заполнить таблицу
                FillTable(cells, templateRowIndex, table.Rows, table.IgnoreZeroes);

                // переход на следующую таблицу
                minRow = templateRowIndex + 1;
            }
        }

        // вернуть результат в нужном формате
        _logger.LogDebug("Формирование выходного файла... в формате {format}", convertToPdf ? "PDF" : "XLSX");
        using var output = new MemoryStream();
        workbook.CalculateFormula();
        workbook.Save(output, convertToPdf ? SaveFormat.Pdf : SaveFormat.Xlsx);
        return output.ToArray();
    }

    public XlsxFiller(
        ILogger<XlsxFiller> logger
        )
    {
        _logger = logger;
    }

    /// <summary>
    /// Замена в одной ячейке
    /// </summary>
    void FillCell(Cell cell, Replacement[] replacements, bool ignoreZeroes)
    {
        var cellValue = cell.Value?.ToString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(cellValue) || !cellValue.Contains("{{") || !cellValue.Contains("}}"))
            return;
        // ищем какие там паттерны
        foreach (var repl in replacements)
        {
            var template = $"{{{repl.Pattern}}}";
            if (!cellValue.Contains(template, StringComparison.OrdinalIgnoreCase))
                continue;

            // на что менять
            var replaceTo = repl.Value?.ToString() ?? string.Empty;
            // если есть опции
            if (repl.Options != null && repl.Options.ToSumString)
            {
                if (decimal.TryParse(replaceTo, out decimal newValue))
                    replaceTo = MoneyConverter.CurrencyToTxt(newValue);
                else
                    _logger.LogWarning("Ошибка преобразования числа {replaceTo} в decimal для строкового представления", replaceTo);
            }
            // игнорирование нулей
            if (ignoreZeroes && decimal.TryParse(replaceTo, out decimal zeroValue) && zeroValue == 0)
                replaceTo = string.Empty;

            // замена
            cellValue = cellValue.Replace(template, replaceTo, StringComparison.OrdinalIgnoreCase);
        }
        // запись нового значения
        cell.PutValue(cellValue);
    }

    /// <summary>
    /// Замена "заголовков" документа XLS
    /// </summary>
    void FillXlsHeaders(Cells cells, Replacement[] replacements)
    {
        for (var r = cells.MinDataRow; r <= cells.MaxDataRow; r++)
            for (var c = cells.MinDataColumn; c <= cells.MaxDataColumn; c++)
                FillCell(cells[r, c], replacements, ignoreZeroes: false);
    }

    /// <summary>
    /// Замена в строке докуметна XLS
    /// </summary>
    void FillXlsRow(int rowIndex, Cells cells, TableRow rowData, bool ignoreZeroValues)
    {
        for (var c = cells.MinDataColumn; c <= cells.MaxDataColumn; c++)
            FillCell(cells[rowIndex, c], rowData.Replacements, ignoreZeroValues);
    }

    /// <summary>
    /// Заполнение таблицы
    /// </summary>
    void FillTable(Cells cells, int templateRowIndex, TableRow[] rows, bool ignoreZeroes)
    {
        var insertIndex = 1;
        foreach (var r in rows)
        {
            // add row
            cells.InsertRow(templateRowIndex + insertIndex);
            cells.CopyRow(cells, templateRowIndex, templateRowIndex + insertIndex);

            // fill row
            FillXlsRow(templateRowIndex + insertIndex, cells, r, ignoreZeroes);

            insertIndex++;
        }
        // удалить строку с шаблоном
        cells.DeleteRow(templateRowIndex);
        // удалить фейк-строку для работы формул
        cells.DeleteRow(templateRowIndex + insertIndex - 1);
    }

    /// <summary>
    /// Поиск строки, содержащей некий шаблон ({{rownumber}})
    /// </summary>
    static int GetXlsRowIndex(Cells cells, string pattern, int minRow = 0)
    {
        var fromRow = minRow == 0 ? cells.MinDataRow : minRow;
        if (fromRow > cells.MaxDataRow)
            return -1;
        for (var r = fromRow; r <= cells.MaxDataRow; r++)
            for (var c = cells.MinDataColumn; c <= cells.MaxDataColumn; c++)
            {
                var cell = cells[r, c];
                var cellValue = cell.Value?.ToString();
                if (!string.IsNullOrWhiteSpace(cellValue) && cellValue.Contains("{{" + pattern + "}}", StringComparison.OrdinalIgnoreCase))
                    return r;
            }
        return -1;
    }
}
