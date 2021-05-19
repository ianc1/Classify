# Classify
A collection of value objects to classify your data to prevent accidental logging of PII and secrets.

## Get Started
Classify can be installed using the Nuget package manager or the dotnet CLI.
```
dotnet add package Classify
```

## Example
```c#
   
    var email = new EMail("my.email@example.com");
    
    # Accessing sensitive values is changed to an explicit operation to ensure its intentional by using the SensitiveValue property. 
    email.SensitiveValue
   
    # Serializing sensitive values to JSON must also be explicit by adding the IncludeSensitiveJsonConverter converter.
    var serializeOptions = new JsonSerializerOptions();
    serializeOptions.Converters.Add(new Classify.JsonSerialization.Microsoft.IncludeSensitiveJsonConverter());
    var json = JsonSerializer.Serialize(email, serializeOptions);    
    
    # To prevent sensitive values from being accidentally logged, the ToString method and the default JSON serializer will return
    # the string "Redacted" instead of the actual sensitive value.   
```