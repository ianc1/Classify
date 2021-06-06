namespace Classify.JsonSerialization.Microsoft
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Concurrent;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Classify.BaseValueObjects;
    
    public class SingleValueObjectConverter : JsonConverter<ISingleValueObject>
    {
        private static readonly ConcurrentDictionary<Type, Type> ConstructorArgumentTypes = new ConcurrentDictionary<Type, Type>();

        public override ISingleValueObject Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            var parameterType = ConstructorArgumentTypes.GetOrAdd(
                objectType,
                t =>
                {
                    var constructorInfo = objectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
                    var parameterInfo = constructorInfo.GetParameters().Single();
                    return parameterInfo.ParameterType;
                });
            
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }
            
            try
            {
                var value = JsonSerializer.Deserialize(ref reader, parameterType);
                
                return value != null ? (ISingleValueObject)Activator.CreateInstance(objectType, value) : null;
            }
            catch (Exception e)
            {
                throw new JsonException(e.InnerException?.Message ?? e.Message);
            }
        }

        public override void Write(Utf8JsonWriter writer, ISingleValueObject value, JsonSerializerOptions options)
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

                case int intValue:
                    writer.WriteNumberValue(intValue);
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
            => typeof(ISingleValueObject).IsAssignableFrom(objectType);

        protected virtual bool IncludeSensitive { get; } = false;
    }
}