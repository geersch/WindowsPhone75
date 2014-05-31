using System;
using Newtonsoft.Json;

namespace StackExchange.Api
{
    public class UnixDateTimeConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(
                    String.Format("Unexpected token parsing date. Expected Integer, got {0}.", reader.TokenType));
            }

            var ticks = (long)reader.Value;

            var d = ticks.FromUnixTime();

            return d;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;

            if (value is DateTime)
            {
                ticks = ((DateTime)value).ToUnixTime();
            }
            else
            {
                throw new Exception("Expected date object value.");
            }

            writer.WriteValue(ticks);
        }
    }
}
