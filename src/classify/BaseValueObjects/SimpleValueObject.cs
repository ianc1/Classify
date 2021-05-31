namespace Classify.BaseValueObjects
{
    using System.Collections.Generic;
    using Classify.JsonSerialization.Newtonsoft;

    [Newtonsoft.Json.JsonConverter(typeof(SimpleValueObjectConverter))]
    public abstract class SimpleValueObject<TValueType> : ValueObject, ISimpleValueObject
    {
        protected SimpleValueObject(TValueType value, string classificationType)
        {
            Value = value;
            ClassificationType = classificationType;
        }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public TValueType Value { get; }
        
        public string ClassificationType { get; }
        
        public virtual object SerializeObject() => Value;

        public override string ToString() => Value.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}