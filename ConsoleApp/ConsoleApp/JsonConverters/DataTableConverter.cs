using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataTableConverter : JsonConverter<DataTable>
    {
        private readonly JsonConverter<DataColumnCollection> _dataColumnCollectionConverter;
        private readonly JsonConverter<DataRowCollection> _dataRowCollectionConverter;

        public DataTableConverter(
            JsonConverter<DataColumnCollection> dataColumnCollectionConverter,
            JsonConverter<DataRowCollection> dataRowCollectionConverter)
        {
            _dataColumnCollectionConverter = dataColumnCollectionConverter;
            _dataRowCollectionConverter = dataRowCollectionConverter;
        }

        public override DataTable? ReadJson(
            JsonReader reader,
            Type objectType,
            DataTable? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            reader.ValidateJsonToken(JsonToken.StartObject);

            reader.ReadAndValidatePropertyName(nameof(existingValue.TableName));

            reader.ReadAndValidateJsonToken(JsonToken.String);

            existingValue.TableName = reader.Value.ToString();

            _dataColumnCollectionConverter.ReadJson(reader, typeof(DataColumnCollection), existingValue.Columns, true, serializer);

            _dataRowCollectionConverter.ReadJson(reader, typeof(DataRowCollection), existingValue.Rows, true, serializer);

            try
            {
                reader.ReadAndValidatePropertyName("DeletedRows");

                reader.ReadAndValidateJsonToken(JsonToken.StartArray);

                reader.ReadAndValidateJsonToken(JsonToken.EndArray);

                reader.ReadAndValidateJsonToken(JsonToken.EndObject);

                return null;
            }
            catch (JsonSerializationException)
            {
                reader.ValidateJsonToken(JsonToken.EndObject);

                return null;
            }
        }

        public override void WriteJson(JsonWriter writer, DataTable? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(value.TableName));

            writer.WriteValue(value.TableName);

            _dataColumnCollectionConverter.WriteJson(writer, value.Columns, serializer);

            _dataRowCollectionConverter.WriteJson(writer, value.Rows, serializer);

            writer.WritePropertyName("DeletedRows");

            writer.WriteStartArray();

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}