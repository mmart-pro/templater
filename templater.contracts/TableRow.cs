namespace templater.contracts;

public class TableRow
{
    /// <summary>
    /// Объекты для замены внутри строки таблицы
    /// </summary>
    public Replacement[] Replacements { get; set; } = Array.Empty<Replacement>();
}
