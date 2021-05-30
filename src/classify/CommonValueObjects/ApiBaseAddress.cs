using System;

namespace Classify.CommonValueObjects
{
    using Classify.BaseValueObjects;
    using Classify.JsonSerialization.Microsoft;

    [System.Text.Json.Serialization.JsonConverter(typeof(RedactSensitiveJsonConverter))] // Todo - Replace with interface converter when supported.
    public class ApiBaseAddress : StringValueObject
    {
        public ApiBaseAddress(string value)
            : base(Validate(value), ClassificationTypes.Public)
        {
        }
        
        private static string Validate(string value)
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out var baseAddress))
            {
                throw new FormatException($"Invalid {nameof(ApiBaseAddress)} format: {value}");
            }

            return baseAddress.AbsoluteUri;
        }
    }
}
