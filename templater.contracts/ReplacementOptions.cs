namespace templater.contracts;

/// <summary>
/// Дополнительные настройки замены
/// </summary>
public class ReplacementOptions
{
    /// <summary>
    /// Преобразовать сумму (дробное число) в строковое представление (ххх рублей хх копеек)
    /// </summary>
    public static ReplacementOptions ToSumStringOption { get; } = new() { ToSumString = true };

    /// <summary>
    /// Преобразовать сумму (дробное число) в строковое представление (ххх рублей хх копеек)
    /// </summary>
    public bool ToSumString { get; set; } = false;
}
