namespace Classify.tests.CommonValueObjects.Person
{
    using System;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Xunit;

    public class PersonalEmailAddressTests
    {
        [Theory]
        [InlineData("Jon.Doe@acme.com")]
        [InlineData("sales@acme.com")]
        [InlineData("sales@acme.co.uk")]
        [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb.com")]
        public void Constructor_should_accept_valid_values(string value)
        {
            var actualValue = new PersonalEmailAddress(value);

            actualValue.SensitiveValue.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("not-an-email")]
        [InlineData("acme.com")]
        [InlineData("@acme.com")]
        [InlineData("name@domain")]
        public void Constructor_should_throw_for_invalid_values(string value)
        {
            Action act = () => new PersonalEmailAddress(value);

            act.Should().Throw<FormatException>().WithMessage("Must be in the format of '<name>@<domain>'.");
        }
    }
}