using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataRowCollectionConverter : JsonConverter<DataRowCollection>
    {
        private readonly JsonConverter<DataRow> _dataRowConverter;

        public DataRowCollectionConverter(JsonConverter<DataRow> dataRowConverter)
        {
            _dataRowConverter = dataRowConverter;
        }

        public override DataRowCollection? ReadJson(
            JsonReader reader,
            Type objectType,
            DataRowCollection? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            _ = existingValue ?? throw new ArgumentNullException(nameof(existingValue));

            reader.ReadAndValidatePropertyName("Rows");

            reader.ReadAndValidateJsonToken(JsonToken.StartArray);

            reader.ReadAndAssert();

            while (reader.TokenType != JsonToken.EndArray)
            {
                var row = existingValue.Add();

                _dataRowConverter.ReadJson(reader, typeof(DataRow), row, true, serializer);

                reader.ReadAndAssert();
            }

            reader.ValidateJsonToken(JsonToken.EndArray);

            return null;
        }

        public override void WriteJson(JsonWriter writer, DataRowCollection? value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Rows");

            writer.WriteStartArray();

            foreach (var row in value.OfType<DataRow>())
            {
                _dataRowConverter.WriteJson(writer, row, serializer);
            }

            writer.WriteEndArray();
        }
    }
}