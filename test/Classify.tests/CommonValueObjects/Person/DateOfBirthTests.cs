namespace Classify.tests.CommonValueObjects.Person
{
    using System;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Xunit;

    public class DateOfBirthTests
    {
        [Theory]
        [InlineData("1979-01-10", 1979, 1, 10)]
        [InlineData("1979-1-9", 1979, 1, 9)]
        public void Constructor_should_accept_valid_values(string value, int expectedYear, int expectedMonth, int expectedDay)
        {
            var actualValue = new DateOfBirth(value);

            actualValue.SensitiveValue.Should().Be(new DateTimeOffset(new DateTime(expectedYear, expectedMonth, expectedDay)));
        }

        [Theory]
        [InlineData("not-a-date")]
        [InlineData("1979-13-1")]
        [InlineData("1979-1-32")]
        public void Constructor_should_throw_for_invalid_values(string value)
        {
            Action act = () => new DateOfBirth(value);

            act.Should().Throw<FormatException>().WithMessage($"Invalid DateOfBirth format: {value}");
        }
    }
}