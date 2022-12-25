using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataRowConverter : JsonConverter<DataRow>
    {
        public override DataRow? ReadJson(
            JsonReader reader,
            Type objectType,
            DataRow? existingValue, 
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            _ = existingValue ?? throw new ArgumentNullException(nameof(existingValue));

            reader.ValidateJsonToken(JsonToken.StartArray);

            reader.Read();

            for (var i = 0; reader.TokenType != JsonToken.EndArray; i++)
            {
                existingValue.SetField(i, reader.Value);

                reader.Read();
            }

            reader.ValidateJsonToken(JsonToken.EndArray);

            return null;
        }

        public override void WriteJson(JsonWriter writer, DataRow? value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            foreach (var e in value.ItemArray)
            {
                writer.WriteValue(e);
            }

            writer.WriteEndArray();
        }
    }
}