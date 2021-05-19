using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Classify.JsonSerialization.Newtonsoft
{
    public class RedactSensitiveJsonConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, Type> ConstructorArgumentTypes = new ConcurrentDictionary<Type, Type>();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var sensitiveValue = (SensitiveValueObject)value;
            
            var include = serializer.Converters.Any(converter => converter.GetType() == typeof(IncludeSensitiveJsonConverter));
            
            serializer.Serialize(writer, include ? sensitiveValue.GetSensitiveValue() : sensitiveValue.ToString());
        }

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
        
        public override bool CanConvert(Type objectType)
        {
            return typeof(SensitiveValueObject).IsAssignableFrom(objectType);
        }
    }
}