namespace Classify
{
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(RedactSensitiveJsonConverter))] // Todo - Replace with interface converter when supported.
    public class EMail : SensitiveValueObject<string>
    {
        public EMail(string value)
            : base(value, ClassificationTypes.PII) {}
    }
}
