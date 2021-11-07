# Classify
A collection of sensitive data types to avoid accidental logging of PII and secrets.

The `PII` and `Secret` types are provided as a direct replacement for `string`.

To avoid accidental logging of these sensitive values the `ToString()` method will return the
`[Redacted]` string, as will attempting to serialize the values to JSON using either the
Newtonsoft or Microsoft serializer.

To access the sensitive values the `SensitiveValue` property is used to make the sensitive nature
of the value more explicit. To serialize the sensitive values the `IncludeSensitiveValuesConverter`
JSON converter must be configured.


## Get Started
Classify can be installed using the Nuget package manager or the dotnet CLI.
```
dotnet add package Classify
```

## Example
Example DTO containing a mix of sensitive and non sensitive properties.
```c#
public class MyUserDto
{
    public string Nickname { get; set; }
    public PII EmailAddress { get; set; }  
    public Secret Password { get; set; }       
}

var user = new MyUserDto
{
    Nickname = "Johnny",
    EmailAddress = new PII("jon.doe@example.com"),
    Password = new Secret("not-a-real-password"),
}

Console.WriteLine(user.EmailAddress)
// [Redacted]

Console.WriteLine(user.EmailAddress.SensitiveValue)
// jon.doe@example.com

Console.WriteLine(JsonSerializer.Serialize(user));
// {
//   "Nickname":"Johnny",
//   "EmailAddress":"[Redacted]",
//   "Password":"[Redacted]"
// }

var serializeOptions = new JsonSerializerOptions();
serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveValuesConverter());

Console.WriteLine(JsonSerializer.Serialize(user, serializeOptions));
// {
//   "Nickname":"Johnny",
//   "EmailAddress":"jon.doe@example.com",
//   "Password":"not-a-real-password"
// }
```


## Example WebApi Usage
[View Source](https://github.com/ianc1/classify/tree/main/example/ExampleWebApi)

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers()
      .AddNewtonsoftJson(options => options.SerializerSettings.Converters
        .Add(new Classify.JsonSerialization.Newtonsoft.IncludeSensitiveValuesConverter()));
}
```