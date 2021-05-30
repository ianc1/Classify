namespace Classify.JsonSerialization.Microsoft
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Classify.BaseValueObjects;
    
    public class RedactSensitiveJsonConverter : JsonConverter<ValueObject>
    {
        private static readonly ConcurrentDictionary<Type, Type> ConstructorArgumentTypes = new ConcurrentDictionary<Type, Type>();

        public override ValueObject Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            var parameterType = ConstructorArgumentTypes.GetOrAdd(
                objectType,
                t =>
                {
                    var constructorInfo = objectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
                    var parameterInfo = constructorInfo.GetParameters().Single();
                    return parameterInfo.ParameterType;
                });

            object objectValue = reader.TokenType switch
            {
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetDecimal(),
                JsonTokenType.True => reader.GetBoolean(),
                JsonTokenType.False => reader.GetBoolean(),
                _ => throw new NotSupportedException($"Unexpected JSON type {reader.TokenType}"),    
            };
           
            return (ValueObject)Activator.CreateInstance(objectType, new[] { objectValue });
        }

        public override void Write(Utf8JsonWriter writer, ValueObject value, JsonSerializerOptions options)
        {
            if (value is SensitiveStringValueObject sensitiveString)
            {
                writer.WriteStringValue(IncludeSensitive() ? sensitiveString.SensitiveValue : sensitiveString.ToString());
                return;
            }

            if (value is SensitiveDecimalValueObject sensitiveDecimal)
            {
                if (IncludeSensitive())
                {
                    writer.WriteNumberValue(sensitiveDecimal.SensitiveValue);
                }
                else
                {
                    writer.WriteStringValue(sensitiveDecimal.ToString());
                }
                return;
            }

            if (value is SensitiveDateTimeOffsetValueObject sensitiveDateTimeOffset)
            {
                writer.WriteStringValue(IncludeSensitive() ? sensitiveDateTimeOffset.ToSensitiveString() : sensitiveDateTimeOffset.ToString());
                return;
            }
            
            if (value is StringValueObject || value is DecimalValueObject || value is BoolValueObject || value is DateTimeOffsetValueObject)
            {
                writer.WriteStringValue(value.ToString()); // todo - this only supports strings.
                return;
            }
            
            throw new NotSupportedException($"Value type {value.GetType()} not supported.");

            bool IncludeSensitive() => options.Converters.Any(converter => converter.GetType() == typeof(IncludeSensitiveJsonConverter));
        }
        
        public override bool CanConvert(Type objectType)
        {
            return typeof(ValueObject).IsAssignableFrom(objectType);
        }
    }
}