namespace Classify.CommonValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(RedactSensitiveJsonConverter))] // Todo - Replace with interface converter when supported.
    public class ApiKey : SensitiveStringValueObject
    {
        public ApiKey(string value)
            : base(value, ClassificationTypes.Secret) {}
    }
}
