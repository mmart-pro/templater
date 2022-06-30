namespace templater.Classes
{
    public class DefaultReplacements
    {
        public KeyValuePair<string, object>[] Replacements;

        public DefaultReplacements()
        {
            // для работы с датой:
            // {{ДАТА}} - теущая дата в формате DD.MM.YYYY
            // {{ДАТАВРЕМЯ}} - текущая дата и время в формате DD.MM.YYYY HH:MM
            // {{ДД}}, {{Д}} - число, с добивкой лидирующего нуля или без
            // {{ММ}}, {{М}} - месяц
            // {{ГГГГ}}, {{ГГ}} - год
            var date = DateTime.Today;
            var dateTime = DateTime.Now;
            Replacements = new KeyValuePair<string, object>[]
            {
                new ("ДАТА", date.ToString("dd.MM.yyyy")),
                new ("ДАТАВРЕМЯ", dateTime.ToString("dd.MM.yyyy HH:mm")),
                new ("Д", date.Day.ToString()),
                new ("ДД", date.ToString("d")),
                new ("М", date.Month.ToString()),
                new ("ММ", date.ToString("MM")),
                new ("ГГ", date.ToString("yy")),
                new ("ГГГГ", date.Year.ToString())
            };
        }
    }
}
