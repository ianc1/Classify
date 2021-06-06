namespace Classify.tests.CommonValueObjects.Person
{
    using System;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Xunit;

    public class GivenNameTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_should_throw_for_invalid_values(string value)
        {
            Action act = () => new GivenName(value);

            act.Should().Throw<FormatException>().WithMessage("Must not be empty.");
        }
    }
}