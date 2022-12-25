using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataRowCollectionConverter : JsonConverter
    {
        private readonly JsonConverter _converter;

        public DataRowCollectionConverter(JsonConverter converter)
        {
            _converter = converter;
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            reader.ReadAndValidatePropertyName("Rows");

            reader.Read();

            reader.ValidateJsonToken(JsonToken.StartArray);

            List<object[]> rows = new List<object[]>();

            while (true)
            {
                var row = (object[])_converter.ReadJson(reader, typeof(object[]), null, serializer);

                if (row == null)
                {
                    break;
                }
                else
                {
                    rows.Add(row);
                }
            }

            return rows.ToArray();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Rows");

            writer.WriteStartArray();

            writer.WriteStartArray();

            foreach(var row in (DataRow[])value)
            {
                _converter.WriteJson(writer, row, serializer);
            }

            writer.WriteEndArray();

            writer.WriteEndArray();
        }
    }
}