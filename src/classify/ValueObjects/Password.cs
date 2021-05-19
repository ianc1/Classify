namespace Classify
{
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(RedactSensitiveJsonConverter))] // Todo - Replace with interface converter when supported.
    public class Password : SensitiveValueObject<string>
    {
        public Password(string value)
            : base(value, ClassificationTypes.Secret) {}
    }
}
