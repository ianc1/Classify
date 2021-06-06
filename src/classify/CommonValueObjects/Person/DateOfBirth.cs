namespace Classify.CommonValueObjects.Person
{
    using System;
    using System.Globalization;
    using Classify.BaseValueObjects;
    using static Classify.Validate;

    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class DateOfBirth : SensitiveValueObject<DateTimeOffset>
    {
        private static string DateFormat = "yyyy-MM-dd";
        
        public DateOfBirth(string value)
            : base(Validate(value), ClassificationTypes.PII) {}

        public override object SerializeObject() => SensitiveValue.ToString(DateFormat);

        private static DateTimeOffset Validate(string value)
        {
            if (!DateTimeOffset.TryParseExact(
                value,
                new[] { DateFormat },
                CultureInfo.InvariantCulture.DateTimeFormat,
                DateTimeStyles.None,
                out var date))
            {
                throw RequiredFormatException(DateFormat);
            }

            return date;
        }
    }
}
