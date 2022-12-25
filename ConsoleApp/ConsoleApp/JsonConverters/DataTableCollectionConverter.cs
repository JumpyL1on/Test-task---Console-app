using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataTableCollectionConverter : JsonConverter<DataTableCollection>
    {
        private readonly JsonConverter<DataTable> _dataTableConverter;

        public DataTableCollectionConverter(JsonConverter<DataTable> dataTableConverter)
        {
            _dataTableConverter = dataTableConverter;
        }

        public override DataTableCollection? ReadJson(
            JsonReader reader,
            Type objectType,
            DataTableCollection? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            _ = existingValue ?? throw new ArgumentNullException(nameof(existingValue));

            reader.ReadAndValidatePropertyName("Tables");

            reader.ReadAndValidateJsonToken(JsonToken.StartArray);

            reader.Read();

            while (reader.TokenType != JsonToken.EndArray)
            {
                var table = existingValue.Add();

                _dataTableConverter.ReadJson(reader, typeof(DataTable), table, true, serializer);

                reader.Read();
            }

            reader.ValidateJsonToken(JsonToken.EndArray);

            return null;
        }

        public override void WriteJson(JsonWriter writer, DataTableCollection? value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Tables");

            writer.WriteStartArray();

            foreach (var table in value.ToArray<DataTable>())
            {
                _dataTableConverter.WriteJson(writer, table, serializer);
            }

            writer.WriteEndArray();
        }
    }
}