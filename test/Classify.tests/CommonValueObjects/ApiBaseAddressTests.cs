namespace Classify.tests.CommonValueObjects
{
    using System;
    using System.Text.Json;
    using Classify.CommonValueObjects;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    public class ApiBaseAddressTests
    {
        private readonly string testValue = "https://test.com/";
    }
}