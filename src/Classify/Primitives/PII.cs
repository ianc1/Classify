namespace Classify.Primitives
{
    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SensitiveValueConverter<PII>))]
    [Newtonsoft.Json.JsonConverter(typeof(Classify.JsonSerialization.Newtonsoft.SensitiveValueConverter<PII>))]
    public sealed class PII : SensitiveValueObject
    {
        public PII(string value) : base(value) { }
    }
}