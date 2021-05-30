namespace Classify.tests.CommonValueObjects.Person
{
    using System.Text.Json;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class FamilyNameTests
    {
        private readonly string TestValue = "Doe";

        [Fact]
        public void Value_should_return_the_non_sensitive_value()
        {
            var valueObject = new FamilyName(TestValue);

            valueObject.Value.Should().Be(TestValue);
        }
        
        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            var valueObject = new FamilyName(TestValue);

            valueObject.ClassificationType.Should().Be("PII");
        }
        
        [Fact]
        public void ToString_should_return_the_non_sensitive_value_as_a_string()
        {
            var valueObject = new FamilyName(TestValue);

            valueObject.ToString().Should().Be(TestValue);
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_the_non_sensitive_value_by_default()
        {
            var json = JsonSerializer.Serialize(new FamilyName(TestValue));
            
            json.Should().Be($"\"{TestValue}\"");
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_the_non_sensitive_value_when_using_the_IncludeSensitive_converter()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var json = JsonSerializer.Serialize(new FamilyName(TestValue));
            
            json.Should().Be($"\"{TestValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_the_non_sensitive_value_by_default()
        {
            var json = JsonConvert.SerializeObject(new FamilyName(TestValue));
            
            json.Should().Be($"\"{TestValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_the_non_sensitive_value_when_using_the_IncludeSensitive_converter()
        {
            var json = JsonConvert.SerializeObject(new FamilyName(TestValue), new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            json.Should().Be($"\"{TestValue}\"");
        }
    }
}