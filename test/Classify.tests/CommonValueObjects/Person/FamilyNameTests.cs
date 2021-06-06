namespace Classify.tests.CommonValueObjects.Person
{
    using System;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Xunit;

    public class FamilyNameTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_should_throw_for_invalid_values(string value)
        {
            Action act = () => new FamilyName(value);

            act.Should().Throw<FormatException>().WithMessage("Must not be empty.");
        }
    }
}