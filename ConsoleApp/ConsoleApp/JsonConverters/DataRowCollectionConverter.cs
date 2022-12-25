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
            reader.ReadAndValidatePropertyName("Rows");

            reader.ReadAndValidateJsonToken(JsonToken.StartArray);

            reader.Read();

            while (reader.TokenType != JsonToken.EndArray)
            {
                var row = existingValue.Add();

                _dataRowConverter.ReadJson(reader, typeof(DataRow), row, true, serializer);

                reader.Read();
            }

            reader.ValidateJsonToken(JsonToken.EndArray);

            return null;
        }

        public override void WriteJson(JsonWriter writer, DataRowCollection? value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Rows");

            writer.WriteStartArray();

            foreach (var row in value.ToArray<DataRow>())
            {
                _dataRowConverter.WriteJson(writer, row, serializer);
            }

            writer.WriteEndArray();
        }
    }
}