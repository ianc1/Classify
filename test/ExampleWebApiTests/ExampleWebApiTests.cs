namespace ExampleWebApiTest
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ExampleWebApi;
    using Microsoft.AspNetCore.Mvc.Testing;
    using RestAssertions;
    using Xunit;

    public class ExampleWebApiTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string AuthToken = "fake-token";

        private readonly HttpClient httpClient;

        public ExampleWebApiTest(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
        }
        
        [Fact]
        public async Task Api_should_return_sensitive_values()
        {
            // act
            var response = await httpClient.TestGet("http://localhost/api/users", AuthToken);

            // assert
            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(new
            {
                Nickname = "Johnny",
                EmailAddress = "jon.doe@example.com",
                Password = "not-a-real-password",
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
            var response = await httpClient.TestPost("http://localhost/api/users", expectedUser, AuthToken);

            // assert
            response.ShouldBe(HttpStatusCode.OK);
            response.ShouldMatchJson(expectedUser);
        }
    }
}
