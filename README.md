# Classify
A collection of value objects to classify your data to prevent accidental logging of PII and secrets.

## Get Started
Classify can be installed using the Nuget package manager or the dotnet CLI.
```
dotnet add package Classify
```

## Example

Sensitive values are created my extending the `SensitiveValueObject` base class or using one of the provided types.
```c#  
var emailAddress = new EmailAddress("my.email@example.com");
var nationalIdentificationNumber = new NationalIdentificationNumber("12345");
var password = new Password("My Secret Password");
```    

Accessing sensitive values is changed to an explicit operation to ensure its intentional by using the SensitiveValue property. 
```c#  
emailAddress.SensitiveValue
```

Serializing sensitive values to JSON must also be explicit by adding the IncludeSensitiveJsonConverter converter.
```c#  
var serializeOptions = new JsonSerializerOptions();
serializeOptions.Converters.Add(new IncludeSensitiveJsonConverter());

var json = JsonSerializer.Serialize(emailAddress, serializeOptions);    
```

To prevent sensitive values from being accidentally logged, the ToString method and the default JSON serializer will return
the string "Redacted" instead of the actual sensitive value.