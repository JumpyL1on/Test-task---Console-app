using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataRowConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            reader.Read();

            if (reader.TokenType == JsonToken.EndArray)
            {
                return null;
            }

            reader.ValidateJsonToken(JsonToken.StartArray);

            reader.Read();

            var values = new List<object>();

            while (reader.TokenType != JsonToken.EndArray)
            {
                values.Add(reader.Value);

                reader.Read();
            }

            return values.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var row = (DataRow)value;

            writer.WriteStartArray();

            foreach (var e in row.ItemArray)
            {
                writer.WriteValue(e);
            }

            writer.WriteEndArray();
        }
    }
}