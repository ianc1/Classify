namespace Classify.tests.ValueObjects
{
    using System.Text.Json;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class PasswordTests
    {
        private readonly Password password = new Password("my.fake.password");
        private readonly TestLogger<PasswordTests> logger;
        
        public PasswordTests(ITestOutputHelper outputHelper)
        {
            logger = new TestLogger<PasswordTests>(outputHelper);
        }

        [Fact]
        public void ToString_should_return_redacted_message()
        {
            password.ToString().Should().Be("Redacted Password");
        }

        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            password.ClassificationType.Should().Be("Secret");
        }

        // ILogger tests

        [Fact]
        public void ILogger_should_return_redacted_message_for_structured_messages()
        {
            logger.LogInformation("Password: {password}", password);

            logger.Messages.Should().Contain("Password: Redacted Password");
        }

        [Fact]
        public void ILogger_should_return_redacted_message_for_string_interpolation_messages()
        {
            logger.LogInformation($"Password: {password}");

            logger.Messages.Should().Contain("Password: Redacted Password");
        }       
        
        [Fact]
        public void SensitiveValue_should_return_the_sensitive_value()
        {
            password.SensitiveValue.Should().Be("my.fake.password");
        }
        
        [Fact]
        public void GetSensitiveValue_should_return_the_sensitive_value_as_type_object()
        {
            password.GetSensitiveValue().Should().Be("my.fake.password");
        }
        
        // Microsoft JsonSerializer tests

        [Fact]
        public void Microsoft_JsonSerializer_should_redact_sensitive_values_by_default()
        {
            var json = JsonSerializer.Serialize(password);
            
            json.Should().Be("\"Redacted Password\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values_only_when_specified()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var json = JsonSerializer.Serialize(password, serializeOptions);
            
            json.Should().Be("\"my.fake.password\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_sensitive_values_by_default()
        {
            var json = "{\"Password\":\"my.fake.password\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var testObject = JsonSerializer.Deserialize<TestObject>(json);
            
            testObject.Password.SensitiveValue.Should().Be("my.fake.password");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_sensitive_values_when_IncludeSensitiveJsonConverter_is_used()
        {
            var json = "{\"Password\":\"my.fake.password\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var testObject = JsonSerializer.Deserialize<TestObject>(json, serializeOptions);
            
            testObject.Password.SensitiveValue.Should().Be("my.fake.password");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
        
        // Newtonsoft JsonConvert tests
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_redact_sensitive_values_by_default()
        {
            var json = JsonConvert.SerializeObject(password);
            
            json.Should().Be("\"Redacted Password\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values_only_when_specified()
        {
            var json = JsonConvert.SerializeObject(password, new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            json.Should().Be("\"my.fake.password\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_sensitive_values_by_default()
        {
            var json = "{\"Password\":\"my.fake.password\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var testObject = JsonConvert.DeserializeObject<TestObject>(json);
            
            testObject.Password.SensitiveValue.Should().Be("my.fake.password");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_sensitive_values_when_IncludeSensitiveJsonConverter_is_used()
        {
            var json = "{\"Password\":\"my.fake.password\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var testObject = JsonConvert.DeserializeObject<TestObject>(json, new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            testObject.Password.SensitiveValue.Should().Be("my.fake.password");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
    }
}