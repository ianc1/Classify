namespace Classify.CommonValueObjects.Person
{
    using Classify.BaseValueObjects;

    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class FamilyName : SensitiveValueObject<string>
    {
        public FamilyName(string value)
            : base(Validate.NotEmpty(value), ClassificationTypes.PII) {}
    }
}
