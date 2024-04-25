namespace Classify.JsonSerialization.Newtonsoft;

using System;
using Classify.Utilities;
using global::Newtonsoft.Json;

public class SensitiveValueConverter<T> : JsonConverter
    where T : SensitiveValueObject
{
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
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

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var sensitiveValue = value as T;
        serializer.Serialize(writer, sensitiveValue?.SensitiveValue);
    }
    
    public override bool CanConvert(Type objectType)
        => typeof(SensitiveValueObject).IsAssignableFrom(objectType);
}