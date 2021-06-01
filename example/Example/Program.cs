namespace Example
{
    using System;
    using System.Text.Json;
    using System.Collections.Generic;
    using Classify;
    using Classify.BaseValueObjects;
    using Classify.CommonValueObjects.Person;

    public class Example
    {
        // Example custom SingleValueObject.
        [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))]
        public class Age : SingleValueObject<int>
        {
            public Age(int value) : base(value, ClassificationTypes.Public) {}
        }
        
        // Example custom SensitiveValueObject.
        [System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))]
        public class SecurityCode : SensitiveValueObject<string>
        {
            public SecurityCode(string value) : base(value, ClassificationTypes.Secret) {}
        }
        
        // Example custom ValueObject containing a mix of sensitive and non sensitive properties.
        public class User : ValueObject
        {
            public Nickname Nickname { get; set; } // Builtin Public
            public Age Age { get; set; } // Custom Public
            public PersonalEmailAddress EmailAddress { get; set; } // Builtin Sensitive PII
            public SecurityCode SecurityCode { get; set; } // Custom Secret
            
            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Nickname;
                yield return Age;
                yield return EmailAddress;
                yield return SecurityCode;
            }
        }
        
        public static void Main()
        {
            var user = new User
            {
                Nickname = new Nickname("Johnny"),
                Age = new Age(27),
                EmailAddress = new PersonalEmailAddress("jon.doe@example.com"),
                SecurityCode = new SecurityCode("Z550"),
            };
            
            Console.WriteLine(user.ToString());
            // {
            //   "Nickname": "Johnny",
            //   "Age": 27,
            //   "EmailAddress": "Redacted PersonalEmailAddress",
            //   "SecurityCode": "Redacted SecurityCode"
            // }

            Console.WriteLine(user.Nickname.Value);
            // Johnny
            
            Console.WriteLine(user.Nickname.ToString());
            // Johnny
            
            Console.WriteLine(user.EmailAddress.SensitiveValue);
            // jon.doe@example.com
            
            Console.WriteLine(user.EmailAddress.ToString());
            // Redacted PersonalEmailAddress
            
            Console.WriteLine(JsonSerializer.Serialize(user));
            // {
            //   "Nickname":"Johnny",
            //   "Age":27,
            //   "EmailAddress":"Redacted PersonalEmailAddress",
            //   "SecurityCode":"Redacted SecurityCode"
            // }
            
            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveValueObjectConverter());
            
            Console.WriteLine(JsonSerializer.Serialize(user, serializeOptions));
            // {
            //   "Nickname":"Johnny",
            //   "Age":27,
            //   "EmailAddress":"jon.doe@example.com",
            //   "SecurityCode":"Z550"
            // }
        }
    }
}