namespace Classify.CommonValueObjects.Person
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class PersonalEmailAddress : SensitiveValueObject<string>
    {
        public PersonalEmailAddress(string value)
            : base(value, ClassificationTypes.PII) {}
    }
}
