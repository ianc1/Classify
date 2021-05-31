namespace Classify.tests.CommonValueObjects.Person
{
    using System.Text.Json;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class PersonalEmailAddressTests
    {
        private readonly PersonalEmailAddress personalEmailAddress = new PersonalEmailAddress("my.PersonalEmailAddress@example.com");
    }
}