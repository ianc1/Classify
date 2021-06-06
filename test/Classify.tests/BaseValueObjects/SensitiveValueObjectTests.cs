namespace Classify.tests.BaseValueObjects
{
    using System;
    using System.Text.Json;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class SensitiveValueObjectTests
    {
        private readonly string testValue = "test-value";

        [Fact]
        public void Constructor_should_throw_for_null_value()
        {
            Action act = () => new TestSensitiveValueObject(null);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'value')");
        }
        
        [Fact]
        public void SensitiveValue_should_return_the_sensitive_value()
        {
            var valueObject = new TestSensitiveValueObject(testValue);

            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            var valueObject = new TestSensitiveValueObject(testValue);

            valueObject.ClassificationType.Should().Be("Secret");
        }
        
        [Fact]
        public void ToString_should_return_the_redacted_message()
        {
            var valueObject = new TestSensitiveValueObject(testValue);

            valueObject.ToString().Should().Be("Redacted TestSensitiveValueObject");
        }
        
        [Fact]
        public void Equals_should_return_true_when_passed_another_instance_with_the_same_value()
        {
            var value1 = new TestSensitiveValueObject("Test Value");
            var value2 = new TestSensitiveValueObject("Test Value");

            value1.Equals(value2).Should().BeTrue();
        }
        
        [Fact]
        public void GetHashCode_should_return_the_same_value_as_another_instance_with_the_same_value()
        {
            var value1 = new TestSensitiveValueObject("Test Value");
            var value2 = new TestSensitiveValueObject("Test Value");

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_redact_sensitive_values_by_default()
        {
            var json = JsonSerializer.Serialize(new TestSensitiveValueObject(testValue));
            
            json.Should().Be("\"Redacted TestSensitiveValueObject\"");
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values_only_when_specified()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveValueObjectConverter());
            
            var json = JsonSerializer.Serialize(new TestSensitiveValueObject(testValue), serializeOptions);
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_non_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<TestSensitiveValueObject>($"\"{testValue}\"");
            
            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<TestSensitiveValueObject>("null");
            
            valueObject.Should().BeNull();
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_throw_for_wrong_types()
        {
            Action act = () => JsonSerializer.Deserialize<TestSensitiveValueObject>("false");

            act.Should().Throw<System.Text.Json.JsonException>()
                .WithMessage("Cannot get the value of a token type 'False' as a string.");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_redact_sensitive_values_by_default()
        {
            var json = JsonConvert.SerializeObject(new TestSensitiveValueObject(testValue));
            
            json.Should().Be("\"Redacted TestSensitiveValueObject\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values_only_when_specified()
        {
            var json = JsonConvert.SerializeObject(new TestSensitiveValueObject(testValue), new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveValueObjectConverter());
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_non_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<TestSensitiveValueObject>($"\"{testValue}\"");
            
            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<TestSensitiveValueObject>("null");
            
            valueObject.Should().BeNull();
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_convert_incorrect_types_to_strings()
        {
            var valueObject = JsonConvert.DeserializeObject<TestSingleValueObject>("false");

            valueObject.Value.Should().Be("False");
        }
    }
}