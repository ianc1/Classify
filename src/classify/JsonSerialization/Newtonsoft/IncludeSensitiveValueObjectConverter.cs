using System;
using Newtonsoft.Json;

namespace Classify.JsonSerialization.Newtonsoft
{
    public class IncludeSensitiveValueObjectConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType) => false;
    }
}