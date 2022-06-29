namespace templater.contracts;

/// <summary>
/// Таблицы для заполнения
/// </summary>
public class Table
{
    /// <summary>
    /// Шаблон, который будет найден для идентификации шаблонной строки в таблице (фигурные скобки добавляются автоматически)
    /// По умолчанию rowNum
    /// </summary>
    public string RowId { get; set; } = "rowNum";

    /// <summary>
    /// Игнорировать нулевые значения (не заполнять нулями)
    /// </summary>
    public bool IgnoreZeroes { get; set; } = false;

    /// <summary>
    /// Строки таблицы, содержащие объекты для замен (структура аналогична replacements)
    /// </summary>
    public TableRow[] Rows { get; set; } = Array.Empty<TableRow>();

    public Table()
    {
    }

    public Table(IEnumerable<IDictionary<string, object>> rows, string rowId = "rowNum", bool ignoreZeroes = false)
    {
        RowId = rowId;
        IgnoreZeroes = ignoreZeroes;
        Rows = rows.Select(x => new TableRow(x)).ToArray();
    }
}
