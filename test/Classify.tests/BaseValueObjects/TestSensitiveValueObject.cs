namespace Classify.tests.BaseValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class TestSensitiveValueObject : SensitiveValueObject<string>
    {
        public TestSensitiveValueObject(string value)
            : base(value, ClassificationTypes.Secret) {}
    }
}