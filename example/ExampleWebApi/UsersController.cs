namespace ExampleWebApi
{
    using System.Collections.Generic;
    using Classify.BaseValueObjects;
    using Classify.CommonValueObjects;
    using Classify.CommonValueObjects.Person;
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
                EmailAddress = new PersonalEmailAddress("jon.doe@example.com"),
                ApiBaseAddress = new ApiBaseAddress("https://test.com"),
            });
        }
    }
    
    // Example custom ValueObject containing a mix of sensitive and non sensitive properties.
    public class User : ValueObject
    {
        public PersonalEmailAddress EmailAddress { get; set; } // Builtin Sensitive PII
        
        public ApiBaseAddress ApiBaseAddress { get; set; } // Builtin Public
            
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EmailAddress;
            yield return ApiBaseAddress;
        }
    }
}