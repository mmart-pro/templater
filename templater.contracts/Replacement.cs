namespace templater.contracts;

/// <summary>
/// Объекты подстановок
/// </summary>
public class Replacement
{
    /// <summary>
    /// Шаблон, который будет заменён на значение, фигурные скобки подставляются автоматически
    /// </summary>
    public string Pattern { get; set; }

    /// <summary>
    /// Значение, на которое заменять шаблон
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Дополнительные настройки замены (не обязательный)
    /// </summary>
    public ReplacementOptions? Options { get; set; }

    public Replacement()
    {
    }

    /// <summary>
    /// Создание замены из пары ключ-значение
    /// </summary>
    public Replacement(KeyValuePair<string, object> value, ReplacementOptions? options = null)
    {
        Pattern = value.Key;
        Value = value.Value;
        Options = options;
    }

    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    /// <param name="pattern">Шаблон, который будет заменён на значение, фигурные скобки подставляются автоматически</param>
    /// <param name="value">Значение, на которое заменять шаблон</param>
    /// <param name="options">Дополнительные настройки замены</param>
    public Replacement(string pattern, object value, ReplacementOptions? options = null)
    {
        Pattern = pattern;
        Value = value;
        Options = options;
    }
}
