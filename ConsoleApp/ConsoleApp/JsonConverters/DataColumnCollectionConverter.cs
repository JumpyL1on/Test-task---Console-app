using ConsoleApp.Enums;
using ConsoleApp.Extensions;
using Newtonsoft.Json;
using System.Data;

namespace ConsoleApp.JsonConverters
{
    internal class DataColumnCollectionConverter : JsonConverter<DataColumnCollection>
    {
        public override DataColumnCollection? ReadJson(
            JsonReader reader,
            Type objectType,
            DataColumnCollection? existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            _ = existingValue ?? throw new ArgumentNullException(nameof(existingValue));

            try
            {
                reader.ReadAndValidatePropertyName("Columns");
            }
            catch (JsonSerializationException)
            {
                return null;
            }

            reader.ReadAndValidateJsonToken(JsonToken.StartArray);

            reader.ReadAndAssert();

            while (reader.TokenType != JsonToken.EndArray)
            {
                var column = reader.Value.ToString().Split(':');

                existingValue.Add(column[0], ResolveColumnType((ColumnType)int.Parse(column[1])));

                reader.ReadAndAssert();
            }

            reader.ValidateJsonToken(JsonToken.EndArray);

            return null;
        }

        public override void WriteJson(JsonWriter writer, DataColumnCollection? value, JsonSerializer serializer)
        {
            writer.WritePropertyName("Columns");

            writer.WriteStartArray();

            foreach(var column in value.ToArray<DataColumn>())
            {
                writer.WriteValue($"{column.ColumnName}:{(int)ResolveType(column.DataType)}");
            }

            writer.WriteEndArray();
        }

        private static Type ResolveColumnType(ColumnType type)
        {
            return type switch
            {
                ColumnType.Empty => typeof(void),
                ColumnType.Object => typeof(object),
                ColumnType.DBNull => typeof(DBNull),
                ColumnType.Boolean => typeof(bool),
                ColumnType.Char => typeof(char),
                ColumnType.SByte => typeof(sbyte),
                ColumnType.Byte => typeof(byte),
                ColumnType.Int16 => typeof(short),
                ColumnType.UInt16 => typeof(ushort),
                ColumnType.Int32 => typeof(int),
                ColumnType.UInt32 => typeof(uint),
                ColumnType.Int64 => typeof(long),
                ColumnType.UInt64 => typeof(ulong),
                ColumnType.Single => typeof(float),
                ColumnType.Double => typeof(double),
                ColumnType.Decimal => typeof(decimal),
                ColumnType.DateTime => typeof(DateTime),
                ColumnType.String => typeof(string),
                ColumnType.Guid => typeof(Guid),
                ColumnType.Array => typeof(Array),
                _ => throw new ArgumentException("Not supported column type"),
            };
        }

        private static ColumnType ResolveType(Type type)
        {

            if (type == typeof(void))
            {
                return ColumnType.Empty;
            }
            else if (type == typeof(DBNull))
            {
                return ColumnType.DBNull;
            }
            else if (type == typeof(bool))
            {
                return ColumnType.Boolean;
            }
            else if (type == typeof(char))
            {
                return ColumnType.Char;
            }
            else if (type == typeof(sbyte))
            {
                return ColumnType.SByte;
            }
            else if (type == typeof(byte))
            {
                return ColumnType.Byte;
            }
            else if (type == typeof(short))
            {
                return ColumnType.Int16;
            }
            else if (type == typeof(ushort))
            {
                return ColumnType.UInt16;
            }
            else if (type == typeof(int))
            {
                return ColumnType.Int32;
            }
            else if (type == typeof(uint))
            {
                return ColumnType.UInt32;
            }
            else if (type == typeof(long))
            {
                return ColumnType.Int64;
            }
            else if (type == typeof(ulong))
            {
                return ColumnType.UInt64;
            }
            else if (type == typeof(float))
            {
                return ColumnType.Single;
            }
            else if (type == typeof(double))
            {
                return ColumnType.Double;
            }
            else if (type == typeof(decimal))
            {
                return ColumnType.Decimal;
            }
            else if (type == typeof(DateTime))
            {
                return ColumnType.DateTime;
            }
            else if (type == typeof(string))
            {
                return ColumnType.String;
            }
            else if (type == typeof(Guid))
            {
                return ColumnType.Guid;
            }
            else if (type == typeof(Array))
            {
                return ColumnType.Array;
            }

            throw new ArgumentException("Not supported type");
        }
    }
}