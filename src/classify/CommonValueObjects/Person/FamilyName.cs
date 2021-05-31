namespace Classify.CommonValueObjects.Person
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class FamilyName : SensitiveValueObject<string>
    {
        public FamilyName(string value)
            : base(value, ClassificationTypes.PII) {}
    }
}
