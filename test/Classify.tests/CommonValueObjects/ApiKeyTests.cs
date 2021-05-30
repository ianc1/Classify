namespace Classify.tests.CommonValueObjects
{
    using System.Text.Json;
    using Classify.CommonValueObjects;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class ApiKeyTests
    {
        private readonly ApiKey ApiKey = new ApiKey("my.fake.ApiKey");
        private readonly TestLogger<ApiKeyTests> logger;
        
        public ApiKeyTests(ITestOutputHelper outputHelper)
        {
            logger = new TestLogger<ApiKeyTests>(outputHelper);
        }

        [Fact]
        public void ToString_should_return_redacted_message()
        {
            ApiKey.ToString().Should().Be("Redacted ApiKey");
        }

        [Fact]
        public void ClassificationType_should_return_correct_classification()
        {
            ApiKey.ClassificationType.Should().Be("Secret");
        }

        // ILogger tests

        [Fact]
        public void ILogger_should_return_redacted_message_for_structured_messages()
        {
            logger.LogInformation("ApiKey: {ApiKey}", ApiKey);

            logger.Messages.Should().Contain("ApiKey: Redacted ApiKey");
        }

        [Fact]
        public void ILogger_should_return_redacted_message_for_string_interpolation_messages()
        {
            logger.LogInformation($"ApiKey: {ApiKey}");

            logger.Messages.Should().Contain("ApiKey: Redacted ApiKey");
        }       
        
        [Fact]
        public void SensitiveValue_should_return_the_sensitive_value()
        {
            ApiKey.SensitiveValue.Should().Be("my.fake.ApiKey");
        }
        
        /*[Fact]
        public void GetSensitiveValue_should_return_the_sensitive_value_as_type_object()
        {
            ApiKey.ToObject().Should().Be("my.fake.ApiKey");
        }*/
        
        // Microsoft JsonSerializer tests

        [Fact]
        public void Microsoft_JsonSerializer_should_redact_sensitive_values_by_default()
        {
            var json = JsonSerializer.Serialize(ApiKey);
            
            json.Should().Be("\"Redacted ApiKey\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values_only_when_specified()
        {
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
            
            var json = JsonSerializer.Serialize(ApiKey, serializeOptions);
            
            json.Should().Be("\"my.fake.ApiKey\"");
        }
        
        // Newtonsoft JsonConvert tests
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_redact_sensitive_values_by_default()
        {
            var json = JsonConvert.SerializeObject(ApiKey);
            
            json.Should().Be("\"Redacted ApiKey\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values_only_when_specified()
        {
            var json = JsonConvert.SerializeObject(ApiKey, new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveJsonConverter());
            
            json.Should().Be("\"my.fake.ApiKey\"");
        }
    }
}