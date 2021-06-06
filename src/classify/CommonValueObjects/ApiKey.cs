namespace Classify.CommonValueObjects
{
    using Classify.BaseValueObjects;

    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class ApiKey : SensitiveValueObject<string>
    {
        public ApiKey(string value)
            : base(Validate.NotEmpty(value), ClassificationTypes.Secret) {}
    }
}
