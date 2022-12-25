using Newtonsoft.Json;

namespace ConsoleApp.Extensions
{
    internal static class JsonReaderExtensions
    {
        public static void ReadAndValidateJsonToken(this JsonReader reader, JsonToken token)
        {
            reader.Read();

            ValidateJsonToken(reader, token);
        }

        public static void ValidateJsonToken(this JsonReader reader, JsonToken token)
        {
            if (reader.TokenType != token)
            {
                throw new JsonSerializationException($"{token} token was expected");
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
                reader.Read();

                ValidatePropertyName(reader, name);
            }
        }

        public static void ValidatePropertyName(this JsonReader reader, string name)
        {
            if (reader.TokenType != JsonToken.PropertyName || reader.Value?.ToString() != name)
            {
                throw new JsonSerializationException($"{JsonToken.PropertyName} token was expected");
            }
        }
    }
}