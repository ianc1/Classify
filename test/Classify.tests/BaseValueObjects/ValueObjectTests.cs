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
        public void ToString_should_return_the_non_sensitive_value_as_a_string()
        {
            testValueObject.ToString().Should().Be("{\r\n"
               + "  \"NativeString\": \"SomeString\",\r\n"
               + "  \"NativeDecimal\": 1.1,\r\n"
               + "  \"NativeBool\": true,\r\n"
               + "  \"NativeDateTimeOffset\": \"2021-12-30T00:00:00+00:00\",\r\n"
               + "  \"NativeUri\": \"/path\",\r\n"
               + "  \"ApiBaseAddress\": \"https://localhost/\",\r\n"
               + "  \"ApiKey\": \"Redacted ApiKey\"\r\n"
               + "}");
        }
        
        [Fact]
        public void Equals_should_return_true_when_passed_another_instance_with_the_same_value()
        {
            var value1 = new TestValueObject { NativeString = "SomeString", NativeDecimal = 1.1M };
            var value2 = new TestValueObject { NativeString = "SomeString", NativeDecimal = 1.1M };

            value1.Equals(value2).Should().BeTrue();
        }
        
        [Fact]
        public void GetHashCode_should_return_the_same_value_as_another_instance_with_the_same_value()
        {
            var value1 = new TestValueObject { NativeString = "SomeString", NativeDecimal = 1.1M };
            var value2 = new TestValueObject { NativeString = "SomeString", NativeDecimal = 1.1M };

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }
        
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