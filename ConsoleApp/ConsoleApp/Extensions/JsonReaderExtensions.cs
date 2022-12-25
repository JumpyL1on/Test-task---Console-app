using Newtonsoft.Json;

namespace ConsoleApp.Extensions
{
    internal static class JsonReaderExtensions
    {
        public static void ReadAndAssert(this JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new JsonSerializationException("Unexpected end when reading JSON.");
            }
        }

        public static void ReadAndValidateJsonToken(this JsonReader reader, JsonToken token)
        {
            reader.ReadAndAssert();

            ValidateJsonToken(reader, token);
        }

        public static void ValidateJsonToken(this JsonReader reader, JsonToken token)
        {
            if (reader.TokenType != token)
            {
                throw new JsonSerializationException($"Unexpected JSON token. Expected {token}, got {reader.TokenType}.");
            }
        }

        public static void ReadAndValidatePropertyName(this JsonReader reader, string name)
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                ValidatePropertyName(reader, name);
            }
            else
            {
                reader.ReadAndAssert();

                ValidatePropertyName(reader, name);
            }
        }

        public static void ValidatePropertyName(this JsonReader reader, string name)
        {
            if (reader.TokenType != JsonToken.PropertyName || reader.Value?.ToString() != name)
            {
                throw new JsonSerializationException($"Unexpected PropertyName. Expected {name}, got {reader.Value}");
            }
        }
    }
}