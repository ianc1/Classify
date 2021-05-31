namespace Classify.CommonValueObjects
{
    using System;
    using Classify.BaseValueObjects;

    [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class ApiBaseAddress : SingleValueObject<Uri>
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
