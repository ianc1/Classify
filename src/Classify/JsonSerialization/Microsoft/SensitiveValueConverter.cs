namespace Classify.JsonSerialization.Microsoft;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Classify.Utilities;

public class SensitiveValueConverter<T> : JsonConverter<T>
    where T : SensitiveValueObject
{
    public override T? Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {           
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        
        try
        {
            var value = JsonSerializer.Deserialize(ref reader, typeof(string));
            
            return value != null ? (T)Activator.CreateInstance(objectType, value) : null;
        }
        catch (Exception e)
        {
            throw new JsonException(e.InnerException?.Message ?? e.Message);
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue((value as T).SensitiveValue);
    }

    public override bool CanConvert(Type objectType)
        => typeof(SensitiveValueObject).IsAssignableFrom(objectType);
}