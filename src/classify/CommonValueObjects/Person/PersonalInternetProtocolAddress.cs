namespace Classify.CommonValueObjects.Person
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(RedactSensitiveJsonConverter))] // Todo - Replace with interface converter when supported.
    public class PersonalInternetProtocolAddress : StringValueObject
    {
        public PersonalInternetProtocolAddress(string value)
            : base(value, ClassificationTypes.PII) {}
    }
}
