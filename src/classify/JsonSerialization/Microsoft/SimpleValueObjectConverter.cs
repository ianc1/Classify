namespace Classify.JsonSerialization.Microsoft
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Classify.BaseValueObjects;
    
    public class SimpleValueObjectConverter : JsonConverter<ISimpleValueObject>
    {
        public override ISimpleValueObject Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            object objectValue = reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetDecimal(),
                JsonTokenType.True => reader.GetBoolean(),
                JsonTokenType.False => reader.GetBoolean(),
                _ => throw new NotSupportedException($"Unexpected JSON type {reader.TokenType}"),    
            };
           
            return (ISimpleValueObject)Activator.CreateInstance(objectType, new[] { objectValue });
        }

        public override void Write(Utf8JsonWriter writer, ISimpleValueObject value, JsonSerializerOptions options)
        {
            if (value.GetType().IsOfGenericType(typeof(SensitiveValueObject<>)) && !IncludeSensitive)
            {
                writer.WriteStringValue(value.ToString());
                return;
            }
            
            switch (value.SerializeObject())
            {
                case string stringValue:
                    writer.WriteStringValue(stringValue);
                    return;

                case long longValue:
                    writer.WriteNumberValue(longValue);
                    return;
                    
                case decimal decimalValue:
                    writer.WriteNumberValue(decimalValue);
                    return;
                    
                case bool boolValue:
                    writer.WriteBooleanValue(boolValue);
                    return;
            }

            throw new NotSupportedException($"Value type {value.GetType()} not supported.");
        }
        
        public override bool CanConvert(Type objectType)
            => typeof(ISimpleValueObject).IsAssignableFrom(objectType);

        protected virtual bool IncludeSensitive { get; } = false;
    }
}