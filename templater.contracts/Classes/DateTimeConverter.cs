using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace templater.contracts.Classes
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.TimeOfDay == TimeSpan.Zero ? value.ToString("dd.MM.yyyy") : value.ToString("dd.MM.yyyy HH:mm"));
        }
    }
}