namespace Classify.CommonValueObjects.Person
{
    using Classify.BaseValueObjects;

    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class PersonalEmailAddress : SensitiveValueObject<string>
    {
        private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string FormatDescription = "<name>@<domain>";
        
        public PersonalEmailAddress(string value)
            : base(Validate.Format(value, EmailRegex, FormatDescription), ClassificationTypes.PII) {}
    }
}
