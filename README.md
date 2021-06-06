# Classify
A collection of value objects to classify your data to prevent accidental logging of PII and secrets.

Person
* [DateOfBirth](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/DateOfBirth.cs) (PII)
* [FamilyName](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/FamilyName.cs) (PII)
* [GivenName](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/GivenName.cs) (PII)
* [Nickname](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/Nickname.cs) (Public)
* [PersonalEmailAddress](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/PersonalEmailAddress.cs) (PII)
* [PersonalInternetProtocolAddress](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/PersonalInternetProtocolAddress.cs) (PII)
* [PersonalTelephoneNumber](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Person/PersonalTelephoneNumber.cs) (PII)

General
* [ApiBaseAddress](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/ApiBaseAddress.cs) (Public)
* [ApiKey](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/ApiKey.cs) (Secret)
* [Password](https://github.com/ianc1/classify/blob/main/src/classify/CommonValueObjects/Password.cs) (Secret)

The values objects are built on three base types, `ValueObject`, `SingleValueObject` and `SensitiveValueObject`.

`ValueObject` is a basic Design Driven Development (DDD) Value Object that all types extend.

`SingleValueObject` is a Value Object that contains only one non null value accessed via its `Value` property.
Newtonsoft and .Net JSON converters are provided and configured by default to convert the Value Object into a JSON
primitive (string, number, boolean or null).

`SensitiveValueObject` is the same as SingleValueObject but for sensitive values. Its value is accessed via its `SensitiveValue`
property to make the sensitive nature of the value more explicit. Sensitive values are not serialized by default,
the `IncludeSensitiveJsonConverter` JSON converter must be specified to serialize sensitive values.


## Get Started
Classify can be installed using the Nuget package manager or the dotnet CLI.
```
dotnet add package Classify
```

## Example
[View Source](https://github.com/ianc1/classify/blob/main/example/ReadmeExample/Program.cs)

Example custom SingleValueObject.
```c#
[System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))]
public class Age : SingleValueObject<int>
{
    public Age(int value) : base(value, ClassificationTypes.Public) {}
}
```

Example custom SensitiveValueObject.
```c#
[System.Text.Json.Serialization.JsonConverter(typeof(Classify.JsonSerialization.Microsoft.SingleValueObjectConverter))]
public class SecurityCode : SensitiveValueObject<string>
{
    public SecurityCode(string value) : base(value, ClassificationTypes.Secret) {}
}
```

Example custom ValueObject containing a mix of sensitive and non sensitive properties.
```c#
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
```

Example usage:
```c#
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
```

## Example WebApi Usage
[View Source](https://github.com/ianc1/classify/tree/main/example/ExampleWebApi)

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
      .AddNewtonsoftJson(options => options.SerializerSettings.Converters
        .Add(new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveValueObjectConverter()));
}
```