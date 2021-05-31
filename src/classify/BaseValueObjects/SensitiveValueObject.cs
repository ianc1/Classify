namespace Classify.BaseValueObjects
{
    using System.Collections.Generic;
    using Classify.JsonSerialization.Newtonsoft;
    
    [Newtonsoft.Json.JsonConverter(typeof(SimpleValueObjectConverter))]
    public abstract class SensitiveValueObject<TValueType> : ValueObject, ISimpleValueObject
    {
        protected SensitiveValueObject(TValueType value, string classificationType)
        {
            SensitiveValue = value;
            ClassificationType = classificationType;
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