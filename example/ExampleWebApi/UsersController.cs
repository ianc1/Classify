namespace ExampleWebApi
{
    using System.Collections.Generic;

    using Classify.Primitives;
    using Microsoft.AspNetCore.Mvc;

    [Route("/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;

        public UsersController(ILogger<UsersController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserAccount user)
        {
            logger.LogInformation($"Received request to create user: {user}"); // Sensitive values will be redacted in log message.

            return Ok(user);
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<UserAccount>> Get()
        {
            return Ok(new[]
            { 
                new UserAccount(
                    Nickname: "Johnny",
                    EmailAddress: new PII("jon.doe@example.com"),
                    Password: new Secret("not-a-real-password")),
            });
        }
    }
    
    public record UserAccount(
        string Nickname,
        PII EmailAddress, // Builtin PII
        Secret Password); // Builtin Secret
}