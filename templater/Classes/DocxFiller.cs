using Aspose.Words;
using Aspose.Words.Replacing;
using templater.contracts;

namespace templater.Classes;

public class DocxFiller
{
    private static readonly FindReplaceOptions _replaceOptions = new()
    {
        MatchCase = false,
        FindWholeWordsOnly = false
    };

    private readonly ILogger<XlsxFiller> _logger;
    private readonly DefaultReplacements _defaultReplacements;

    public DocxFiller(
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
        var document = new Document(input);

        // заполнить таблицы
        foreach (var templateTable in contract.Tables)
        {
            var table = FindTable(document, templateTable.RowId);
            // нашли нужную для заполнения таблицу
            if (table != null)
                FillTable(table, templateTable);
        }

        // заполнить подстановки
        _logger.LogDebug("Заполнение подстановок в документе...");
        foreach (var item in contract.Replacements.AsEnumerable().Union(_defaultReplacements.Replacements))
            document.Range.Replace("{{" + item.Key + "}}", item.Value.ToString(), _replaceOptions);

        // вернуть результат в нужном формате
        _logger.LogDebug("Формирование выходного файла... в формате {format}", convertToPdf ? "PDF" : "DOCX");
        using var output = new MemoryStream();
        document.Save(output, convertToPdf ? SaveFormat.Pdf : SaveFormat.Docx);
        return output.ToArray();
    }

    /// <summary>
    /// Поиск таблицы в документе по rowId
    /// </summary>
    static Aspose.Words.Tables.Table? FindTable(Document document, string rowId)
    {
        var docTables = document.GetChildNodes(NodeType.Table, true);
        foreach (var t in docTables)
        {
            var docTable = (Aspose.Words.Tables.Table)t;
            var cells = docTable.Rows[1].Cells;
            for (var i = 0; i < cells.Count; i++)
            {
                // проверить соответствует ли ячейка искомому RowNumber
                var cellValue = cells[i].ToString(SaveFormat.Text); //.Replace("\r", "").Replace("\n", "").Trim();
                if (string.IsNullOrEmpty(cellValue) || cellValue.Length < 5 || !cellValue.Contains("{{" + rowId + "}}", StringComparison.OrdinalIgnoreCase))
                    continue;
                // нашли нужную для заполнения таблицу
                return docTable;
            }
        }
        return null;
    }

    void FillTable(Aspose.Words.Tables.Table table, Table contractTable)
    {
        var index = 1;
        foreach (var replacements in contractTable.Rows)
        {
            table.Rows.Add(table.Rows[1].Clone(true));
            FillTableRow(table.Rows[index + 1].Cells, replacements, contractTable.IgnoreZeroes);
            index++;
        }
        table.Rows.RemoveAt(1);
    }

    void FillTableRow(Aspose.Words.Tables.CellCollection rowCells, IDictionary<string, object> replacements, bool ignoreZeroes)
    {
        for (var i = 0; i < rowCells.Count; i++)
        {
            var cellValue = rowCells[i].ToString(SaveFormat.Text);//.Replace("\r", "").Replace("\n", "").Trim();
            if (string.IsNullOrEmpty(cellValue) || cellValue.Length < 5 || !cellValue.Contains("{{") || !cellValue.Contains("}}"))
                continue;

            var repls = replacements.AsEnumerable().Union(_defaultReplacements.Replacements);
            var modified = false;
            foreach (var repl in repls)
            {
                var template = "{{" + repl.Key + "}}";

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
                modified = true;
            }
            if (modified)
                PutCellValue(rowCells[i], cellValue);
        }
    }

    static void PutCellValue(Aspose.Words.Tables.Cell cell, string value)
    {
        var run = (Run)cell.FirstParagraph.Runs[0].Clone(true);
        run.Text = value;
        cell.RemoveAllChildren();
        cell.EnsureMinimum();
        cell.Paragraphs[0].AppendChild(run);
    }
}
