namespace Classify.tests.CommonValueObjects.Person
{
    using System;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Xunit;

    public class PersonalInternetProtocolAddressTests
    {
        [Theory]
        [InlineData("0.0.0.0")]
        [InlineData("255.255.255.255")]
        public void Constructor_should_accept_valid_values(string value)
        {
            var actualValue = new PersonalInternetProtocolAddress(value);

            actualValue.SensitiveValue.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("-1.-1.-1.-1")]
        [InlineData("0.0.0.0.0")]
        [InlineData("0.0.0")]
        [InlineData("0.0")]
        [InlineData("0")]
        public void Constructor_should_throw_for_invalid_values(string value)
        {
            Action act = () => new PersonalInternetProtocolAddress(value);

            act.Should().Throw<FormatException>().WithMessage("Must be in the format of '<0-255>.<0-255>.<0-255>.<0-255>'.");
        }
    }
}