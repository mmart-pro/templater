namespace templater.contracts;

/// <summary>
/// Дополнительные настройки замены
/// </summary>
public class ReplacementOptions
{
    /// <summary>
    /// Преобразовать сумму (дробное число) в строковое представление (ххх рублей хх копеек)
    /// </summary>
    public static ReplacementOptions ToSumStringOption { get; }

    /// <summary>
    /// Преобразовать целое число в строковое представление
    /// </summary>
    public static ReplacementOptions ToWordsStringOption { get; }

    /// <summary>
    /// Преобразовать сумму (дробное число) в строковое представление (ххх рублей хх копеек)
    /// </summary>
    public bool ToSumString { get; set; } = false;

    /// <summary>
    /// Преобразовать целое число в строковое представление
    /// </summary>
    public bool ToWordsString { get; set; } = false;

    static ReplacementOptions()
    {
        ToSumStringOption = new() { ToSumString = true };
        ToWordsStringOption = new() { ToWordsString = true };
    }
}
