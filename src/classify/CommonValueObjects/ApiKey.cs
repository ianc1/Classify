namespace Classify.CommonValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class ApiKey : SensitiveValueObject<string>
    {
        public ApiKey(string value)
            : base(value, ClassificationTypes.Secret) {}
    }
}
