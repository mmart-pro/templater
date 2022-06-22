namespace templater;

public class AppSettings
{
    public string DB_PATH { get; set; }

    public string DOC_PATH { get; set; }

    public string TEMPLATE_PATH { get; set; }

    /// <summary>
    /// Дней хранения документов
    /// </summary>
    public int DOC_DAYS_KEEP { get; set; }
}
