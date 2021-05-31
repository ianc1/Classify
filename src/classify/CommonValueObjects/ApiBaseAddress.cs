namespace Classify.CommonValueObjects
{
    using System;
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(SimpleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class ApiBaseAddress : SimpleValueObject<Uri>
    {
        public ApiBaseAddress(string value)
            : base(Validate(value), ClassificationTypes.Public) {}

        public override object SerializeObject() => Value.AbsoluteUri;

        private static Uri Validate(string value)
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out var baseAddress))
            {
                throw new FormatException($"Invalid {nameof(ApiBaseAddress)} format: {value}");
            }

            return baseAddress;
        }
    }
}
