namespace ExampleWebApiTest
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using RestAssertions;
    using Xunit;

    public class ExampleWebApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private const string AuthToken = "fake-token";

        private readonly HttpClient httpClient;

        public ExampleWebApiTest(WebApplicationFactory<Program> factory)
        {
            httpClient = factory.CreateClient();
        }
        
        [Fact]
        public async Task Api_should_return_sensitive_values()
        {
            // act
            var response = await httpClient.TestGet("http://localhost/users", AuthToken);

            // assert
            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(new[]
            {
                new
                {
                    Nickname = "Johnny",
                    EmailAddress = "jon.doe@example.com",
                    Password = "not-a-real-password",
                },
            });
        }
        
        [Fact]
        public async Task Api_should_accept_sensitive_values_and_return_them()
        {
            // arrange
            var expectedUser = new
            {
                Nickname = "Johnny",
                EmailAddress = "jon.doe@example.com",
                Password = "not-a-real-password",
            };
            
            // act
            var response = await httpClient.TestPost("http://localhost/users", expectedUser, AuthToken);

            // assert
            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(expectedUser);
        }
    }
}
