namespace Classify.tests.CommonValueObjects
{
    using System;
    using System.Text.Json;
    using Classify.CommonValueObjects;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class ApiBaseAddressTests
    {
        private readonly string TestValue = "https://test.com/";

        [Fact]
        public void Value_should_return_the_non_sensitive_value()
        {
            var valueObject = new ApiBaseAddress(TestValue);

            valueObject.Value.Should().Be(TestValue);
        }
        
        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            var valueObject = new ApiBaseAddress(TestValue);

            valueObject.ClassificationType.Should().Be("Public");
        }
        
        [Fact]
        public void ToString_should_return_the_non_sensitive_value_as_a_string()
        {
            var valueObject = new ApiBaseAddress(TestValue);

            valueObject.ToString().Should().Be(TestValue);
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_the_non_sensitive_value_by_default()
        {
            var json = JsonSerializer.Serialize(new ApiBaseAddress(TestValue));
            
            json.Should().Be($"\"{TestValue}\"");
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_serialize_the_non_sensitive_value_when_using_the_IncludeSensitive_converter()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var json = JsonSerializer.Serialize(new ApiBaseAddress(TestValue));
            
            json.Should().Be($"\"{TestValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_the_non_sensitive_value_by_default()
        {
            var json = JsonConvert.SerializeObject(new ApiBaseAddress(TestValue));
            
            json.Should().Be($"\"{TestValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_serialize_the_non_sensitive_value_when_using_the_IncludeSensitive_converter()
        {
            var json = JsonConvert.SerializeObject(new ApiBaseAddress(TestValue), new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            json.Should().Be($"\"{TestValue}\"");
        }
        
    }
}