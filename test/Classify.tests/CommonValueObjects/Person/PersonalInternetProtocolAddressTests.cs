namespace Classify.tests.CommonValueObjects.Person
{
    using System.Text.Json;
    using Classify.CommonValueObjects.Person;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class PersonalInternetProtocolAddressTests
    {
        private readonly string testValue = "10.1.10.2";
    }
}