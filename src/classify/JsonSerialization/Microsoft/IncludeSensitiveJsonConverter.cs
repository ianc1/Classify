using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Classify.JsonSerialization.Microsoft
{
    public class IncludeSensitiveJsonConverter : JsonConverter<SensitiveValueObject>
    {
        public override SensitiveValueObject Read(
            ref Utf8JsonReader reader,
            Type objectType,
            JsonSerializerOptions options)
        {
            object objectValue = reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetDouble(),
                JsonTokenType.True => reader.GetBoolean(),
                JsonTokenType.False => reader.GetBoolean(),
                _ => throw new NotSupportedException($"Unexpected JSON type {reader.TokenType}"),
            };
            
            return (SensitiveValueObject)Activator.CreateInstance(objectType, new[] { objectValue });
        }
        
        public override void Write(
            Utf8JsonWriter writer,
            SensitiveValueObject sensitiveValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(sensitiveValue.GetSensitiveValue().ToString());
        }
        
        public override bool CanConvert(Type objectType)
        {
            return typeof(SensitiveValueObject).IsAssignableFrom(objectType);
        }
    }
}