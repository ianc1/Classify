namespace Classify.tests.BaseValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class TestSimpleValueObject : SimpleValueObject<string>
    {
        public TestSimpleValueObject(string value)
            : base(value, ClassificationTypes.Public) {}

        public override object SerializeObject() => Value;
    }
}