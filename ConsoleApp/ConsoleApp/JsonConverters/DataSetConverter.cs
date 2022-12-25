using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataSetConverter : JsonConverter<DataSet>
    {
        private readonly JsonConverter<DataTableCollection> _dataTableCollectionConverter;

        public DataSetConverter(JsonConverter<DataTableCollection> dataTableCollectionConverter)
        {
            _dataTableCollectionConverter = dataTableCollectionConverter;
        }

        public override DataSet? ReadJson(
            JsonReader reader,
            Type objectType,
            DataSet? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            reader.ValidateJsonToken(JsonToken.StartObject);

            var dataSet = existingValue ?? new DataSet();

            reader.ReadAndValidatePropertyName(nameof(dataSet.DataSetName));

            reader.ReadAndValidateJsonToken(JsonToken.String);

            dataSet.DataSetName = reader.Value.ToString();

            _dataTableCollectionConverter.ReadJson(reader, typeof(DataTableCollection), dataSet.Tables, true, serializer);

            reader.ReadAndValidateJsonToken(JsonToken.EndObject);

            return dataSet;
        }

        public override void WriteJson(JsonWriter writer, DataSet? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            writer.WritePropertyName(nameof(value.DataSetName));

            writer.WriteValue(value.DataSetName);

            _dataTableCollectionConverter.WriteJson(writer, value.Tables, serializer);

            writer.WriteEndObject();
        }
    }
}