namespace Classify.tests.BaseValueObjects
{
    using System;
    using System.Text.Json;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class SimpleValueObjectTests
    {
        private readonly string testValue = "Sparky";

        [Fact]
        public void Constructor_should_throw_for_null_value()
        {
            Action act = () => new TestSingleValueObject(null);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'value')");
        }
        
        [Fact]
        public void Value_should_return_the_sensitive_value()
        {
            var valueObject = new TestSingleValueObject(testValue);

            valueObject.Value.Should().Be(testValue);
        }
        
        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            var valueObject = new TestSingleValueObject(testValue);

            valueObject.ClassificationType.Should().Be("Public");
        }
        
        [Fact]
        public void ToString_should_return_the_value()
        {
            var valueObject = new TestSingleValueObject(testValue);

            valueObject.ToString().Should().Be(testValue);
        }
        
        [Fact]
        public void Equals_should_return_true_when_passed_another_instance_with_the_same_value()
        {
            var value1 = new TestSingleValueObject("Test Value");
            var value2 = new TestSingleValueObject("Test Value");

            value1.Equals(value2).Should().BeTrue();
        }
        
        [Fact]
        public void GetHashCode_should_return_the_same_value_as_another_instance_with_the_same_value()
        {
            var value1 = new TestSingleValueObject("Test Value");
            var value2 = new TestSingleValueObject("Test Value");

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_single_values_by_default()
        {
            var json = JsonSerializer.Serialize(new TestSingleValueObject(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_single_values_when_the_sensitive_converter_is_specified()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveValueObjectConverter());
            
            var json = JsonSerializer.Serialize(new TestSingleValueObject(testValue), serializeOptions);
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_non_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<TestSingleValueObject>($"\"{testValue}\"");
            
            valueObject.Value.Should().Be(testValue);
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<TestSingleValueObject>("null");
            
            valueObject.Should().BeNull();
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_throw_for_wrong_types()
        {
            Action act = () => JsonSerializer.Deserialize<TestSingleValueObject>("false");

            act.Should().Throw<System.Text.Json.JsonException>()
                .WithMessage("Cannot get the value of a token type 'False' as a string.");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_single_values_by_default()
        {
            var json = JsonConvert.SerializeObject(new TestSingleValueObject(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_single_values_when_the_sensitive_converter_is_specifiedd()
        {
            var json = JsonConvert.SerializeObject(new TestSingleValueObject(testValue), new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveValueObjectConverter());
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_non_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<TestSingleValueObject>($"\"{testValue}\"");
            
            valueObject.Value.Should().Be(testValue);
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<TestSingleValueObject>("null");
            
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