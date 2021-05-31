namespace Classify.tests.BaseValueObjects
{
    using System.Text.Json;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class SimpleValueObjectTests
    {
        private readonly string testValue = "Sparky";

        [Fact]
        public void Value_should_return_the_non_sensitive_value()
        {
            var valueObject = new TestSimpleValueObject(testValue);

            valueObject.Value.Should().Be(testValue);
        }
        
        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            var valueObject = new TestSimpleValueObject(testValue);

            valueObject.ClassificationType.Should().Be("Public");
        }
        
        [Fact]
        public void ToString_should_return_the_non_sensitive_value_as_a_string()
        {
            var valueObject = new TestSimpleValueObject(testValue);

            valueObject.ToString().Should().Be(testValue);
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_the_non_sensitive_value_by_default()
        {
            var json = JsonSerializer.Serialize(new TestSimpleValueObject(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_the_non_sensitive_value_when_using_the_IncludeSensitive_converter()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveValueObjectConverter());
            
            var json = JsonSerializer.Serialize(new TestSimpleValueObject(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_the_non_sensitive_value_by_default()
        {
            var json = JsonConvert.SerializeObject(new TestSimpleValueObject(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_the_non_sensitive_value_when_using_the_IncludeSensitive_converter()
        {
            var json = JsonConvert.SerializeObject(new TestSimpleValueObject(testValue), new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveValueObjectConverter());
            
            json.Should().Be($"\"{testValue}\"");
        }
    }
}