namespace templater.contracts;

public class TableRow
{
    /// <summary>
    /// Объекты для замены внутри строки таблицы
    /// </summary>
    public Replacement[] Replacements { get; set; } = Array.Empty<Replacement>();

    public TableRow()
    {
    }

    public TableRow(IDictionary<string, object> values)
    {
        Replacements = values.Select(v => new Replacement(v)).ToArray();
    }
}
