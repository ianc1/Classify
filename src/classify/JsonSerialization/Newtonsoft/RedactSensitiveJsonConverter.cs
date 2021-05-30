using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Classify.BaseValueObjects;
using Newtonsoft.Json;

namespace Classify.JsonSerialization.Newtonsoft
{
    public class RedactSensitiveJsonConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, Type> ConstructorArgumentTypes = new ConcurrentDictionary<Type, Type>();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var parameterType = ConstructorArgumentTypes.GetOrAdd(
                objectType,
                t =>
                {
                    var constructorInfo = objectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
                    var parameterInfo = constructorInfo.GetParameters().Single();
                    return parameterInfo.ParameterType;
                });

            var value = serializer.Deserialize(reader, parameterType);
            return Activator.CreateInstance(objectType, new[] { value });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is SensitiveStringValueObject sensitiveString)
            {
                WriteSensitiveValue(sensitiveString.SensitiveValue, sensitiveString.ToString());
                return;
            }

            if (value is SensitiveDecimalValueObject sensitiveDecimal)
            {
                WriteSensitiveValue(sensitiveDecimal.SensitiveValue, sensitiveDecimal.ToString());
                return;
            }
            
            if (value is SensitiveDateTimeOffsetValueObject sensitiveDateTimeOffset)
            {
                WriteSensitiveValue(sensitiveDateTimeOffset.ToSensitiveString(), sensitiveDateTimeOffset.ToString());
                return;
            }
            
            if (value is StringValueObject stringValue)
            {
                serializer.Serialize(writer, stringValue.Value);
                return;
            }
            
            if (value is DecimalValueObject decimalValue)
            {
                serializer.Serialize(writer, decimalValue.Value);
                return;
            }
            
            if (value is BoolValueObject boolValue)
            {
                serializer.Serialize(writer, boolValue.Value);
                return;
            }
            
            if (value is DateTimeOffsetValueObject dateTimeOffsetValue)
            {
                serializer.Serialize(writer, dateTimeOffsetValue.Value);
                return;
            }

            throw new NotSupportedException($"Value type {value.GetType()} not supported.");

            void WriteSensitiveValue(object sensitiveValue, object redactedValue)
            {
                var includeSensitive = serializer.Converters.Any(converter => converter.GetType() == typeof(IncludeSensitiveJsonConverter));

                serializer.Serialize(writer, includeSensitive ? sensitiveValue : redactedValue);
            }     
        }
        
        public override bool CanConvert(Type objectType)
        {
            return typeof(ValueObject).IsAssignableFrom(objectType);
        }
    }
}