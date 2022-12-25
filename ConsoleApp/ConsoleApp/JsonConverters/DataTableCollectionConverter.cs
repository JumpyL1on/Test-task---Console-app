using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataTableCollectionConverter : JsonConverter<DataTable[]>
    {
        private readonly JsonConverter<DataTable> _converter;

        public DataTableCollectionConverter(JsonConverter<DataTable> converter)
        {
            _converter = converter;
        }

        public override DataTable[]? ReadJson(JsonReader reader, Type objectType, DataTable[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            reader.ReadAndValidatePropertyName("Tables");

            reader.Read();

            reader.ValidateJsonToken(JsonToken.StartArray);

            var tables = new List<DataTable>();

            while (true)
            {
                var table = _converter.ReadJson(reader, typeof(DataTable), null, false, serializer);

                if (table == null)
                {
                    break;
                }
                else
                {
                    tables.Add(table);
                }
            }

            return tables.ToArray();
        }

        public override void WriteJson(JsonWriter writer, DataTable[]? value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Tables");

            writer.WriteStartArray();

            foreach (var table in value)
            {
                _converter.WriteJson(writer, table, serializer);
            }

            writer.WriteEndArray();
        }
    }
}