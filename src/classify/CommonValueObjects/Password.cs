namespace Classify.CommonValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class Password : SensitiveValueObject<string>
    {
        public Password(string value)
            : base(value, ClassificationTypes.Secret) {}
    }
}
