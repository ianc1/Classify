namespace Classify.tests.Primitives
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using Classify.Primitives;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class PIITests
    {
        private readonly ITestOutputHelper outputHelper;
        private readonly string testValue = "test-value";

        public PIITests(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        [Fact]
        public void Constructor_should_throw_for_null_value()
        {
            Action act = () => new PII(null);

            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'value')");
        }
        
        [Fact]
        public void SensitiveValue_should_return_the_sensitive_value()
        {
            var valueObject = new PII(testValue);

            valueObject.SensitiveValue.Should().Be(testValue);
        }      
       
        [Fact]
        public void ToString_should_return_the_redacted_message()
        {
            var valueObject = new PII(testValue);

            valueObject.ToString().Should().Be("[Redacted]");
        }

        [Fact]
        public void ToString_should_return_the_redacted_message_when_used_in_a_logger()
        {
            var logger = new TestLogger<PIITests>(outputHelper);
            var valueObject = new PII(testValue);

            logger.LogInformation($"PII: {valueObject}");

            logger.Messages[0].Should().Be("PII: [Redacted]");
        }
        
        [Fact]
        public void Equals_should_return_true_when_passed_another_instance_with_the_same_value()
        {
            var value1 = new PII("Test Value");
            var value2 = new PII("Test Value");

            value1.Equals(value2).Should().BeTrue();

            (value1 == value2).Should().BeTrue();
            (value1 != value2).Should().BeFalse();
        }

        [Fact]
        public void Equals_should_return_false_when_passed_another_instance_with_a_different_value()
        {
            var value1 = new PII("Test Value");
            var value2 = new PII("Different Value");

            value1.Equals(value2).Should().BeFalse();

            (value1 == value2).Should().BeFalse();
            (value1 != value2).Should().BeTrue();
        }

        [Fact]
        public void Equals_should_return_false_when_passed_null()
        {
            var value1 = new PII(testValue);

            value1.Equals(null).Should().BeFalse();
        }
        
        [Fact]
        public void GetHashCode_should_return_the_same_value_as_another_instance_with_the_same_value()
        {
            var value1 = new PII("Test Value");
            var value2 = new PII("Test Value");

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_should_return_a_different_value_when_passed_another_instance_with_a_different_value()
        {
            var value1 = new PII("Test Value");
            var value2 = new PII("Test Value");

            value1.GetHashCode().Should().Be(value2.GetHashCode());
        }

        [Fact]
        public void Microsoft_JsonSerializer_should_include_sensitive_values()
        {           
            var json = JsonSerializer.Serialize(new PII(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_non_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<PII>($"\"{testValue}\"");
            
            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_deserialize_null_values()
        {
            var valueObject = JsonSerializer.Deserialize<PII>("null");
            
            valueObject.Should().BeNull();
        }
        
        [Fact]
        public void Microsoft_JsonSerializer_should_throw_for_wrong_types()
        {
            Action act = () => JsonSerializer.Deserialize<PII>("false");

            act.Should().Throw<System.Text.Json.JsonException>()
                .WithMessage("Cannot get the value of a token type 'False' as a string.");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_include_sensitive_values()
        {
            var json = JsonConvert.SerializeObject(new PII(testValue));
            
            json.Should().Be($"\"{testValue}\"");
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_non_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<PII>($"\"{testValue}\"");
            
            valueObject.SensitiveValue.Should().Be(testValue);
        }
        
        [Fact]
        public void Newtonsoft_JsonConvert_should_deserialize_null_values()
        {
            var valueObject = JsonConvert.DeserializeObject<PII>("null");
            
            valueObject.Should().BeNull();
        }

        [Fact]
        public void Newtonsoft_JsonConvert_should_throw_for_wrong_types()
        {
            Action act = () => JsonConvert.DeserializeObject<PII>("false");

            act.Should().Throw<Newtonsoft.Json.JsonException>()
                .WithMessage("Cannot get the value of a token type 'False' as a string.");
        }

        [Fact]
        public void PII_types_should_be_sortable()
        {
            // arrange
            var unordered = new[] { new PII("b"), new PII("a"), new PII("d"), new PII("c") };

            // act
            var ascending = unordered.Order().ToList();
            var descending = unordered.OrderDescending().ToList();

            // assert
            ascending.Should().Equal([new PII("a"), new PII("b"), new PII("c"), new PII("d")]);

            descending.Should().Equal([new PII("d"), new PII("c"), new PII("b"), new PII("a")]);
        }

        [Fact]
        public void CompareTo_should_be_sortable()
        {
            // arrange
            var a = new PII("a");
            var b = new PII("b");

            // / act
            var sameValue = a.CompareTo(new PII("a"));
            var sameReference = a.CompareTo(a);

            var before = a.CompareTo(b);
            var after = b.CompareTo(a);
            var nullAfter = a.CompareTo(default(PII));

            // assert
            sameValue.Should().Be(0);
            sameReference.Should().Be(0);
            before.Should().Be(-1);
            after.Should().Be(1);
            nullAfter.Should().Be(1);
        }

        [Fact]
        public void Operators_should_be_sortable()
        {
            // arrange
            var a = new PII("a");
            var b = new PII("b");

            // act
            var sameValue = a == new PII("a");
#pragma warning disable CS1718 // Comparison made to same variable
            var sameReference = a == a;
#pragma warning restore CS1718 // Comparison made to same variable

            var differentValue = a != b;

            var before = a < b;
            var beforeOrEqual = a <= b;
            var after = b > a;
            var afterOrEqual = b >= a;
            var nullAfter = a > default(PII);

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
        public void Formatting_a_record_DTO_using_ToString_should_redact_PII_properties()
        {
            // arrange
            var dto = new TestDto(1, new PII("John Smith"));

            // act
            var dtoString = dto.ToString();

            // assert
            dtoString.Should().Be("TestDto { Id = 1, name = [Redacted] }");
        }
            private record TestDto(int Id, PII name);
    }
}