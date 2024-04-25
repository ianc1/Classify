namespace Classify.Utilities;

using System.Collections.Generic;

public abstract class SensitiveValueObject : ValueObject
{
    public SensitiveValueObject(string value)
    {
        SensitiveValue = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string SensitiveValue { get; }

    public override string ToString() => "[Redacted]";

    protected override IEnumerable<IComparable?> GetEqualityComponents()
    {
        yield return SensitiveValue;
    }
}