namespace Classify.Primitives
{
    using System;

    public abstract class SensitiveValueObject
    {
        public SensitiveValueObject(string value)
        {
            SensitiveValue = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string SensitiveValue { get; }
        
        public override string ToString() => "[Redacted]";

        public override bool Equals(object obj)
            => SensitiveValue.Equals((obj as SensitiveValueObject)?.SensitiveValue);

        public override int GetHashCode()
            => SensitiveValue.GetHashCode();
    }
}