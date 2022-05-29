using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Classify.Primitives;
using Newtonsoft.Json;

namespace Classify.JsonSerialization.Newtonsoft
{
    public class SensitiveValueConverter<T> : JsonConverter
        where T : SensitiveValueObject
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException("Cannot get the value of a token type 'False' as a string.");
            }
            
            try
            {
                var value = serializer.Deserialize(reader, typeof(string));
                
                return value != null ? Activator.CreateInstance(objectType, value) : null;
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(e.InnerException?.Message ?? e.Message);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var includeSensitiveValues = serializer.Converters.Any(converter =>
                converter.GetType() == typeof(IncludeSensitiveValuesConverter));

            if (includeSensitiveValues)
            {
                var sensitiveValue = value as T;
                serializer.Serialize(writer, sensitiveValue.SensitiveValue);
                return;
            }
            
            serializer.Serialize(writer, value.ToString());
        }
        
        public override bool CanConvert(Type objectType)
            => typeof(SensitiveValueObject).IsAssignableFrom(objectType);
    }
}