namespace Classify.BaseValueObjects
{
    using System;
    using System.Collections.Generic;
    
    [Newtonsoft.Json.JsonConverter(typeof(Classify.JsonSerialization.Newtonsoft.SingleValueObjectConverter))]
    public abstract class SensitiveValueObject<TValueType> : ValueObject, ISingleValueObject
    {
        protected SensitiveValueObject(TValueType value, string classificationType)
        {
            SensitiveValue = value ?? throw new ArgumentNullException(nameof(value));
            ClassificationType = classificationType ?? throw new ArgumentNullException(nameof(classificationType));
        }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TValueType SensitiveValue { get; }
        
        public string ClassificationType { get; }

        public virtual object SerializeObject() => SensitiveValue;
        
        public override string ToString() => $"Redacted {this.GetType().Name}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return SensitiveValue;
        }
    }
}