using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Classify.BaseValueObjects;
using Newtonsoft.Json;

namespace Classify.JsonSerialization.Newtonsoft
{
    public class SingleValueObjectConverter : JsonConverter
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
            
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            
            try
            {
                var value = serializer.Deserialize(reader, parameterType);
                
                return value != null ? Activator.CreateInstance(objectType, value) : null;
            }
            catch (Exception e)
            {
                throw new JsonSerializationException(e.InnerException?.Message ?? e.Message);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var includeSensitive = serializer.Converters.Any(converter =>
                converter.GetType() == typeof(IncludeSensitiveValueObjectConverter));

            if (value.GetType().IsOfGenericType(typeof(SensitiveValueObject<>)) && !includeSensitive)
            {
                serializer.Serialize(writer, value.ToString());
                return;
            }
            
            if (value is ISingleValueObject simpleValueObject)
            {
                switch (simpleValueObject.SerializeObject())
                {
                    case string stringValue:
                        serializer.Serialize(writer, stringValue);
                        return;

                    case int intValue:
                        serializer.Serialize(writer, intValue);
                        return;
                    
                    case long longValue:
                        serializer.Serialize(writer, longValue);
                        return;
                    
                    case decimal decimalValue:
                        serializer.Serialize(writer, decimalValue);
                        return;
                    
                    case bool boolValue:
                        serializer.Serialize(writer, boolValue);
                        return;
                }
            }

            throw new NotSupportedException($"Value type {value.GetType()} not supported.");
        }
        
        public override bool CanConvert(Type objectType)
            => typeof(ISingleValueObject).IsAssignableFrom(objectType);
    }
}