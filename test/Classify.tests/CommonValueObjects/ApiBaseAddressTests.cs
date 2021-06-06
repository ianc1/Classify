namespace Classify.tests.CommonValueObjects
{
    using System;
    using Classify.CommonValueObjects;
    using FluentAssertions;
    using Xunit;

    public class ApiBaseAddressTests
    {
        [Theory]
        [InlineData("http://host/")]
        [InlineData("https://host/")]
        [InlineData("https://host/path")]
        [InlineData("HTTPS://host/path")]
        public void Constructor_should_accept_valid_values(string value)
        {
            var actualValue = new ApiBaseAddress(value);

            actualValue.Value.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("not-a-uri")]
        [InlineData("ftp://host/")]
        public void Constructor_should_throw_for_invalid_values(string value)
        {
            Action act = () => new ApiBaseAddress(value);

            act.Should().Throw<FormatException>().WithMessage("Must be in the format of 'http[s]://<host>/'.");
        }
    }
}