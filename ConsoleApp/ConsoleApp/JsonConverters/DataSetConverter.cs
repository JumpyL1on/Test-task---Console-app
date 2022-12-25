using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataSetConverter : JsonConverter<DataSet>
    {
        private readonly JsonConverter _dataTableCollectionConverter;

        public DataSetConverter(JsonConverter converter)
        {
            _dataTableCollectionConverter = converter;
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

            var ds = existingValue ?? new DataSet();

            reader.ReadAndValidatePropertyName(nameof(ds.DataSetName));

            reader.ReadAndValidateJsonToken(JsonToken.String);

            ds.DataSetName = reader.Value.ToString();

            var tables = _dataTableCollectionConverter.ReadJson(reader, typeof(DataTable[]), null, serializer);

            ds.Tables.AddRange((DataTable[])tables);

            reader.ReadAndValidateJsonToken(JsonToken.EndObject);

            reader.Read();

            return ds;
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

            _dataTableCollectionConverter.WriteJson(writer, value.Tables.ToArray<DataTable>(), serializer);

            writer.WriteEndObject();
        }
    }
}