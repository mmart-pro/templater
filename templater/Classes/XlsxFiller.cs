using Aspose.Cells;
using System.Text.Json;
using templater.contracts;

namespace templater.Classes;

public class XlsxFiller
{
    private readonly ILogger<XlsxFiller> _logger;
    private readonly DefaultReplacements _defaultReplacements;

    public XlsxFiller(
        ILogger<XlsxFiller> logger,
        DefaultReplacements defaultReplacements
        )
    {
        _logger = logger;
        _defaultReplacements = defaultReplacements;
    }

    public byte[] Fill(byte[] template, Template contract, bool convertToPdf)
    {
        using var input = new MemoryStream(template);

        var workbook = new Workbook(input);
        var cells = workbook.Worksheets[0].Cells;

        // заполнить подстановки
        _logger.LogDebug("Заполнение подстановок в документе...");
        FillXlsHeaders(cells, contract.Replacements);

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

    /// <summary>
    /// Замена в одной ячейке
    /// </summary>
    void FillCell(Cell cell, IDictionary<string, object> replacements, bool ignoreZeroes)
    {
        var cellValue = cell.Value?.ToString() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(cellValue) || cellValue.Length < 5 || !cellValue.Contains("{{") || !cellValue.Contains("}}"))
            return;

        var repls = replacements.AsEnumerable().Union(_defaultReplacements.Replacements);
        foreach (var repl in repls)
        {
            var template = "{{" + repl.Key + "}}";
            // если полное совпадение с шаблоном
            if (cellValue.Equals(template, StringComparison.OrdinalIgnoreCase))
            {
                ReplaceCellValue(cell, repl.Value ?? string.Empty, ignoreZeroes);
                return;
            }

            // вообще нет такого шаблона
            if (!cellValue.Contains(template, StringComparison.OrdinalIgnoreCase))
                continue;

            // на что менять
            var replaceTo = repl.Value?.ToString() ?? string.Empty;
            // игнорирование нулей
            if (ignoreZeroes && decimal.TryParse(replaceTo, out var zeroValue) && zeroValue == 0)
                replaceTo = string.Empty;

            // замена
            cellValue = cellValue.Replace(template, replaceTo, StringComparison.OrdinalIgnoreCase);
        }
        cell.PutValue(cellValue);
    }

    /// <summary>
    /// Замена "заголовков" документа XLS
    /// </summary>
    void FillXlsHeaders(Cells cells, Dictionary<string, object> replacements)
    {
#warning вот тут наверное надо собрать новый словарь в lowerCase, тогда FullCell сможет без перебора обходится
        for (var r = cells.MinDataRow; r <= cells.MaxDataRow; r++)
            for (var c = cells.MinDataColumn; c <= cells.MaxDataColumn; c++)
                FillCell(cells[r, c], replacements, ignoreZeroes: false);
    }

    /// <summary>
    /// Замена в строке докуметна XLS
    /// </summary>
    void FillXlsRow(int rowIndex, Cells cells, IDictionary<string, object> rowData, bool ignoreZeroValues)
    {
#warning вот тут наверное надо собрать новый словарь в lowerCase, тогда FullCell сможет без перебора обходится
        for (var c = cells.MinDataColumn; c <= cells.MaxDataColumn; c++)
            FillCell(cells[rowIndex, c], rowData, ignoreZeroValues);
    }

    /// <summary>
    /// Заполнение таблицы
    /// </summary>
    void FillTable(Cells cells, int templateRowIndex, IDictionary<string, object>[] rows, bool ignoreZeroes)
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

    static void ReplaceCellValue(Cell cell, object value, bool ignoreZeroes)
    {
        if (value is not JsonElement || ((JsonElement)value).ValueKind != JsonValueKind.Number)
        {
            cell.PutValue(value.ToString());
            return;
        }
        // если это число
        var je = (JsonElement)value;
        if (je.TryGetInt64(out var i))
            cell.PutValue(ignoreZeroes && i == 0 ? string.Empty : i);
        else if (je.TryGetDouble(out var d))
            cell.PutValue(ignoreZeroes && d == 0 ? string.Empty : d);
        else
            cell.PutValue(string.Empty);
    }
}
