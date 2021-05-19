using System;

namespace Classify.tests.ValueObjects
{
    using System.Text.Json;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class EMailTests
    {
        private readonly EMail email = new EMail("my.email@example.com");
        
        [Fact]
        public void ToString_should_return_redacted_message()
        {
            email.ToString().Should().Be("Redacted EMail");
        }
        
        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            email.ClassificationType.Should().Be("PII");
        }
        
        [Fact]
        public void SensitiveValue_should_return_the_sensitive_value()
        {
            email.SensitiveValue.Should().Be("my.email@example.com");
        }
        
        [Fact]
        public void GetSensitiveValue_should_return_the_sensitive_value_as_type_object()
        {
            email.GetSensitiveValue().Should().Be("my.email@example.com");
        }
        
        // Microsoft_JsonSerializer

        [Fact]
        public void Microsoft_JsonSerializer_should_redact_sensitive_values_by_default()
        {
            var json = JsonSerializer.Serialize(email);
            
            json.Should().Be("\"Redacted EMail\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values_only_when_specified()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var json = JsonSerializer.Serialize(email, serializeOptions);
            
            json.Should().Be("\"my.email@example.com\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_sensitive_values_by_default()
        {
            var json = "{\"Email\":\"Jimmy@domain.local\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var testObject = JsonSerializer.Deserialize<TestObject>(json);
            
            testObject.Email.SensitiveValue.Should().Be("Jimmy@domain.local");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_sensitive_values_when_IncludeSensitiveJsonConverter_is_used()
        {
            var json = "{\"Email\":\"Jimmy@domain.local\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var testObject = JsonSerializer.Deserialize<TestObject>(json, serializeOptions);
            
            testObject.Email.SensitiveValue.Should().Be("Jimmy@domain.local");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
        
        // Newtonsoft_JsonConvert
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_redact_sensitive_values_by_default()
        {
            var json = JsonConvert.SerializeObject(email);
            
            json.Should().Be("\"Redacted EMail\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values_only_when_specified()
        {
            var json = JsonConvert.SerializeObject(email, new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            json.Should().Be("\"my.email@example.com\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_sensitive_values_by_default()
        {
            var json = "{\"Email\":\"Jimmy@domain.local\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var testObject = JsonConvert.DeserializeObject<TestObject>(json);
            
            testObject.Email.SensitiveValue.Should().Be("Jimmy@domain.local");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_sensitive_values_when_IncludeSensitiveJsonConverter_is_used()
        {
            var json = "{\"Email\":\"Jimmy@domain.local\",\"TestString\":\"SomeString\",\"TestNumber\": 1.1}";
            
            var testObject = JsonConvert.DeserializeObject<TestObject>(json, new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            testObject.Email.SensitiveValue.Should().Be("Jimmy@domain.local");
            testObject.TestString.Should().Be("SomeString");
            testObject.TestNumber.Should().Be(1.1);
        }
    }
}
