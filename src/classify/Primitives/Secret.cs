namespace Classify.Primitives
{
    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SensitiveValueConverter<Secret>))]
    [Newtonsoft.Json.JsonConverter(typeof(Classify.JsonSerialization.Newtonsoft.SensitiveValueConverter<Secret>))]
    public sealed class Secret : SensitiveValueObject
    {
        public Secret(string value) : base(value) { }
    }
}