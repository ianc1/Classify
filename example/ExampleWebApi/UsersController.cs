namespace ExampleWebApi
{
    using Classify.Primitives;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            return Ok(user);
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new User
            {
                Nickname = "Johnny",
                EmailAddress = new PII("jon.doe@example.com"),
                Password = new Secret("not-a-real-password"),
            });
        }
    }
    
    public class User
    {
        public string Nickname { get; set; }

        public PII EmailAddress { get; set; } // Builtin PII
        
        public Secret Password { get; set; } // Builtin Secret       
    }
}