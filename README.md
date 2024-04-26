# Classify
A collection of data types to provide safe handling of sensitive PII and secrets.

Two types `PII` and `Secret` are provided as a direct replacements for `string`. They
are intended to be used through your code base, from API DTOs to AppSettings and Domain
ValueObjects. Anywhere you would have used a string before.

To access the sensitive value stored in these types a `SensitiveValue` property is provided
to make it explicit about the sensitive nature of the data being accessed.

The `ToString()` method on these types will return the string `[Redacted]` to help avoid 
accidentally leaking sensitive values. For example you can safely log a DTO and its sensitive
values will be automatically sanitized.

Another benefit of classifying the sensitive data using these type is that it makes it easier
to identity what PII and secret data is being used in your application.

## Get Started
Classify can be installed using the Nuget package manager or the dotnet CLI.
```
dotnet add package Classify
```

## Example
Example DTO containing a mix of sensitive and non sensitive properties.
```c#
public record MyUserDto(
    string Nickname,
    PII EmailAddress,
    Secret Password);

var user = new MyUserDto(
    Nickname: "Johnny",
    EmailAddress: new PII("jon.doe@example.com"),
    Password: new Secret("not-a-real-password"));

Console.WriteLine(user)
// MyUserDto { Nickname = Johnny, EmailAddress = [Redacted], Password = [Redacted] }

Console.WriteLine(user.EmailAddress.SensitiveValue)
// jon.doe@example.com

Console.WriteLine(JsonSerializer.Serialize(user, serializeOptions));
// {
//   "Nickname":"Johnny",
//   "EmailAddress":"jon.doe@example.com",
//   "Password":"not-a-real-password"
// }
```

## Swagger
If you use Swagger to document your API, you will need to add the below two mappings to
your Swagger configuration to display the sensitive data types as strings.

```
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<PII>(() => new OpenApiSchema { Type = "string" });
    options.MapType<Secret>(() => new OpenApiSchema { Type = "string" });
});
```