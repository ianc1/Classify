namespace Classify.CommonValueObjects.Person
{
    using Classify.BaseValueObjects;

    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class PersonalInternetProtocolAddress : SensitiveValueObject<string>
    {
        private const string IPRegex = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
        private const string FormatDescription = "<0-255>.<0-255>.<0-255>.<0-255>";
        
        public PersonalInternetProtocolAddress(string value)
            : base(Validate.Format(value, IPRegex, FormatDescription), ClassificationTypes.PII) {}
    }
}
