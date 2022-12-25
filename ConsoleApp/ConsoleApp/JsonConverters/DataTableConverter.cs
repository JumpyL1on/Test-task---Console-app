using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataTableConverter : JsonConverter<DataTable>
    {
        private readonly JsonConverter<DataColumn[]> _columnsConverter;
        private readonly JsonConverter _rowsConverter;

        public DataTableConverter(JsonConverter<DataColumn[]> columnsConverter, JsonConverter rowsConverter)
        {
            _columnsConverter = columnsConverter;
            _rowsConverter = rowsConverter;
        }

        public override DataTable? ReadJson(JsonReader reader, Type objectType, DataTable? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            reader.Read();

            if (reader.TokenType == JsonToken.EndArray)
            {
                return null;
            }

            reader.ValidateJsonToken(JsonToken.StartObject);

            var table = existingValue ?? new DataTable();

            reader.ReadAndValidatePropertyName(nameof(table.TableName));

            reader.Read();

            table.TableName = reader.Value.ToString();

            table.Columns.AddRange(_columnsConverter.ReadJson(reader, typeof(DataColumn[]), null, false, serializer));

            foreach (var row in (object[][])_rowsConverter.ReadJson(reader, typeof(object[]), null, serializer))
            {
                table.Rows.Add(row);
            }

            reader.Read();

            if (reader.TokenType == JsonToken.PropertyName)
            {
                reader.Read();

                reader.ValidateJsonToken(JsonToken.StartArray);

                reader.Read();

                reader.ValidateJsonToken(JsonToken.EndArray);

                reader.Read();
            }

            return table;
        }

        public override void WriteJson(JsonWriter writer, DataTable? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(value.TableName));

            writer.WriteValue(value.TableName);

            _columnsConverter.WriteJson(writer, value.Columns.ToArray<DataColumn>(), serializer);

            _rowsConverter.WriteJson(writer, value.Rows.ToArray<DataRow>(), serializer);

            writer.WritePropertyName("DeletedRows");

            writer.WriteStartArray();

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}