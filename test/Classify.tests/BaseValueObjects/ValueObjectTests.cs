namespace Classify.tests.BaseValueObjects
{
    using System;
    using System.Text.Json;
    using Newtonsoft.Json;
    using Classify.CommonValueObjects;
    using FluentAssertions;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class ValueObjectTests
    {
        private readonly TestValueObject testValueObject = new TestValueObject
        {
            NativeString = "SomeString",
            NativeDecimal = 1.1M,
            NativeBool = true,
            NativeUri = new Uri("/path", UriKind.Relative),
            NativeDateTimeOffset = new DateTimeOffset(new DateTime(2021, 12, 30)),
            ApiBaseAddress = new ApiBaseAddress("https://localhost/"),
            ApiKey = new ApiKey("secret-key"),
        };
        
        [Fact]
        public void Microsoft_JsonSerializer_should_redact_sensitive_values_by_default()
        {
            var json = JsonSerializer.Serialize(testValueObject);
            
            json.Should().Be("{\"NativeString\":\"SomeString\","
                             + "\"NativeDecimal\":1.1,"
                             + "\"NativeBool\":true,"
                             + "\"NativeDateTimeOffset\":\"2021-12-30T00:00:00+00:00\","
                             + "\"NativeUri\":\"/path\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"Redacted ApiKey\"}");
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values_only_when_specified()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveValueObjectConverter());
            
            var json = JsonSerializer.Serialize(testValueObject, serializeOptions);
            
            json.Should().Be("{\"NativeString\":\"SomeString\","
                             + "\"NativeDecimal\":1.1,"
                             + "\"NativeBool\":true,"
                             + "\"NativeDateTimeOffset\":\"2021-12-30T00:00:00+00:00\","
                             + "\"NativeUri\":\"/path\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"secret-key\"}");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_redact_sensitive_values_by_default()
        {
            var json = JsonConvert.SerializeObject(testValueObject);
            
            json.Should().Be("{\"NativeString\":\"SomeString\","
                             + "\"NativeDecimal\":1.1,"
                             + "\"NativeBool\":true,"
                             + "\"NativeDateTimeOffset\":\"2021-12-30T00:00:00+00:00\","
                             + "\"NativeUri\":\"/path\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"Redacted ApiKey\"}");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values_only_when_specified()
        {
            var json = JsonConvert.SerializeObject(testValueObject, new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveValueObjectConverter());
            
            json.Should().Be("{\"NativeString\":\"SomeString\","
                             + "\"NativeDecimal\":1.1,"
                             + "\"NativeBool\":true,"
                             + "\"NativeDateTimeOffset\":\"2021-12-30T00:00:00+00:00\","
                             + "\"NativeUri\":\"/path\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"secret-key\"}");
        }
    }
}