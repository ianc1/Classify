using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Classify.JsonSerialization.Microsoft
{
    public class IncludeSensitiveValuesConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) => false;
    }
}