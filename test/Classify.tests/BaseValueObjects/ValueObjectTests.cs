namespace Classify.tests.BaseValueObjects
{
    using System;
    using System.Text.Json;
    using Newtonsoft.Json;
    using Classify.CommonValueObjects;
    using Classify.CommonValueObjects.Person;
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
            EmailAddress = new PersonalEmailAddress("test@example.com"),
            ApiKey = new ApiKey("secret-key"),
            StartTime = new StartTime(new DateTime(2021, 12, 30)),
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
               + "  \"EmailAddress\": \"Redacted PersonalEmailAddress\",\r\n"
               + "  \"ApiBaseAddress\": \"https://localhost/\",\r\n"
               + "  \"ApiKey\": \"Redacted ApiKey\",\r\n"
               + "  \"StartTime\": \"2021-12-30T00:00:00.0000000\"\r\n"
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
                             + "\"EmailAddress\":\"Redacted PersonalEmailAddress\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"Redacted ApiKey\","
                             + "\"StartTime\":\"2021-12-30T00:00:00.0000000\"}");
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
                             + "\"EmailAddress\":\"test@example.com\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"secret-key\","
                             + "\"StartTime\":\"2021-12-30T00:00:00.0000000\"}");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<TestValueObject>("{\"NativeString\":null,"
                 + "\"NativeDecimal\":null,"
                 + "\"NativeBool\":null,"
                 + "\"NativeDateTimeOffset\":null,"
                 + "\"NativeUri\":null,"
                 + "\"ApiBaseAddress\":null,"
                 + "\"ApiKey\":null,"
                 + "\"StartTime\":null}");
            
            valueObject.Should().NotBeNull();
            valueObject.NativeString.Should().BeNull();
            valueObject.NativeDecimal.Should().BeNull();
            valueObject.NativeBool.Should().BeNull();
            valueObject.NativeDateTimeOffset.Should().BeNull();
            valueObject.NativeUri.Should().BeNull();
            valueObject.ApiBaseAddress.Should().BeNull();
            valueObject.ApiKey.Should().BeNull();
            valueObject.StartTime.Should().BeNull();
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_throw_when_values_are_the_wrong_type()
        {
            Action act = () => JsonSerializer.Deserialize<TestValueObject>("{"
                + "\"EmailAddress\":false,"
                + "\"ApiBaseAddress\":false,"
                + "\"ApiKey\":false}");
            
            // TODO - Exception only contains the first validation error. Unlike the WebApi ModelState validation which contains all errors.
            act.Should().Throw<Exception>().WithMessage("Cannot get the value of a token type 'False' as a string.");
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
                             + "\"EmailAddress\":\"Redacted PersonalEmailAddress\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"Redacted ApiKey\","
                             + "\"StartTime\":\"2021-12-30T00:00:00.0000000\"}");
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
                             + "\"EmailAddress\":\"test@example.com\","
                             + "\"ApiBaseAddress\":\"https://localhost/\","
                             + "\"ApiKey\":\"secret-key\","
                             + "\"StartTime\":\"2021-12-30T00:00:00.0000000\"}");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<TestValueObject>("{\"NativeString\":null,"
                 + "\"NativeDecimal\":null,"
                 + "\"NativeBool\":null,"
                 + "\"NativeDateTimeOffset\":null,"
                 + "\"NativeUri\":null,"
                 + "\"ApiBaseAddress\":null,"
                 + "\"ApiKey\":null,"
                 + "\"StartTime\":null}");
            
            valueObject.Should().NotBeNull();
            valueObject.NativeString.Should().BeNull();
            valueObject.NativeDecimal.Should().BeNull();
            valueObject.NativeBool.Should().BeNull();
            valueObject.NativeDateTimeOffset.Should().BeNull();
            valueObject.NativeUri.Should().BeNull();
            valueObject.ApiBaseAddress.Should().BeNull();
            valueObject.ApiKey.Should().BeNull();
            valueObject.StartTime.Should().BeNull();
        }

        [Fact]
        public void Newtonsoft_JsonConvert_should_throw_when_values_are_the_wrong_type()
        {
            Action act = () => JsonConvert.DeserializeObject<TestValueObject>("{"
                + "\"EmailAddress\":false,"
                + "\"ApiBaseAddress\":false,"
                + "\"ApiKey\":false}");

            // TODO - Exception only contains the first validation error. Unlike the WebApi ModelState validation which contains all errors.
            act.Should().Throw<JsonSerializationException>().WithMessage("Must be in the format of '<name>@<domain>'.");
        }
    }
}