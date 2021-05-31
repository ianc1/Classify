namespace Classify.tests.BaseValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class TestSingleValueObject : SingleValueObject<string>
    {
        public TestSingleValueObject(string value)
            : base(value, ClassificationTypes.Public) {}

        public override object SerializeObject() => Value;
    }
}