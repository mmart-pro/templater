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
                new ("ДД", date.Day.ToString("D2")),
                new ("М", date.Month.ToString()),
                new ("ММ", date.Month.ToString("D2")),
                new ("МЕСЯЦ", MonthToString(date.Month)),
                new ("ГГ", (date.Year-2000).ToString()),
                new ("ГГГГ", date.Year.ToString())
            };
        }

        static string MonthToString(int month)
        {
            var monthes = new[]
            {
                "января",
                "февраля",
                "марта",
                "апреля",
                "мая",
                "июня",
                "июля",
                "августа",
                "сентября",
                "октября",
                "ноября",
                "декабря"
            };
            return monthes[month - 1];
        }
    }
}
