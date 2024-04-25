namespace Classify.tests.Primitives
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using Classify.Primitives;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class SecretTests
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly string testValue = "test-value";

        public SecretTests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void Constructor_should_throw_for_null_value()
        {
            Action act = () => new Secret(null);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'value')");
        }
        
        [Fact]
        public void SensitiveValue_should_return_the_sensitive_value()
        {
            var valueObject = new Secret(testValue);

            valueObject.SensitiveValue.Should().Be(testValue);
        }      
       
        [Fact]
        public void ToString_should_return_the_redacted_message()
        {
            var valueObject = new Secret(testValue);

            valueObject.ToString().Should().Be("[Redacted]");
        }

        [Fact]
        public void ToString_should_return_the_redacted_message_when_used_in_a_logger()
        {
            var logger = new TestLogger<SecretTests>(outputHelper);
            var valueObject = new Secret(testValue);

            logger.LogInformation($"Secret: {valueObject}");

            logger.Messages[0].Should().Be("Secret: [Redacted]");
        }
        
        [Fact]
        public void Equals_should_return_true_when_passed_another_instance_with_the_same_value()
        {
            var value1 = new Secret("Test Value");
            var value2 = new Secret("Test Value");

            value1.Equals(value2).Should().BeTrue();
        }

        [Fact]
        public void Equals_should_return_false_when_passed_another_instance_with_a_different_value()
        {
            var value1 = new Secret("Test Value");
            var value2 = new Secret("Different Value");

            value1.Equals(value2).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_return_false_when_passed_null()
        {
            var value1 = new Secret(testValue);

            value1.Equals(null).Should().BeFalse();
        }
        
        [Fact]
        public void GetHashCode_should_return_the_same_value_as_another_instance_with_the_same_value()
        {
            var value1 = new Secret("Test Value");
            var value2 = new Secret("Test Value");

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_should_return_a_different_value_when_passed_another_instance_with_a_different_value()
        {
            var value1 = new Secret("Test Value");
            var value2 = new Secret("Test Value");

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values()
        {
            var json = JsonSerializer.Serialize(new Secret(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_non_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<Secret>($"\"{testValue}\"");
            
            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<Secret>("null");
            
            valueObject.Should().BeNull();
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_throw_for_wrong_types()
        {
            Action act = () => JsonSerializer.Deserialize<Secret>("false");

            act.Should().Throw<System.Text.Json.JsonException>()
                .WithMessage("Cannot get the value of a token type 'False' as a string.");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values()
        {
            var json = JsonConvert.SerializeObject(new Secret(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_non_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<Secret>($"\"{testValue}\"");
            
            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<Secret>("null");
            
            valueObject.Should().BeNull();
        }

        [Fact]
        public void Newtonsoft_JsonConvert_should_throw_for_wrong_types()
        {
            Action act = () => JsonConvert.DeserializeObject<Secret>("false");

            act.Should().Throw<Newtonsoft.Json.JsonException>()
                .WithMessage("Cannot get the value of a token type 'False' as a string.");
        }

        [Fact]
        public void Secret_types_should_be_sortable()
        {
            // arrange
            var unordered = new[] { new Secret("b"), new Secret("a"), new Secret("d"), new Secret("c") };

            // act
            var ascending = unordered.Order().ToList();
            var descending = unordered.OrderDescending().ToList();

            // assert
            ascending.Should().Equal([new Secret("a"), new Secret("b"), new Secret("c"), new Secret("d")]);

            descending.Should().Equal([new Secret("d"), new Secret("c"), new Secret("b"), new Secret("a")]);
        }

        [Fact]
        public void CompareTo_should_be_sortable()
        {
            // arrange
            var a = new Secret("a");
            var b = new Secret("b");

            // act
            var sameValue = a == new Secret("a");
#pragma warning disable CS1718 // Comparison made to same variable
            var sameReference = a == a;
#pragma warning restore CS1718 // Comparison made to same variable

            var differentValue = a != b;

            var before = a < b;
            var beforeOrEqual = a <= b;
            var after = b > a;
            var afterOrEqual = b >= a;
            var nullAfter = a > default(Secret);

            // assert
            sameValue.Should().BeTrue();
            sameReference.Should().BeTrue();
            differentValue.Should().BeTrue();
            before.Should().BeTrue();
            beforeOrEqual.Should().BeTrue();
            after.Should().BeTrue();
            afterOrEqual.Should().BeTrue();
            nullAfter.Should().BeTrue();
        }

        [Fact]
        public void Formatting_a_record_DTO_using_ToString_should_redact_Secret_properties()
        {
            // arrange
            var dto = new TestDto(1, new Secret("My Test Secret"));

            // act
            var dtoString = dto.ToString();

            // assert
            dtoString.Should().Be("TestDto { Id = 1, Secret = [Redacted] }");
        }

        private record TestDto(int Id, Secret Secret);
    }
}