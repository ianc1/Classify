namespace Classify.BaseValueObjects
{
    using System.Collections.Generic;

    [Newtonsoft.Json.JsonConverter(typeof(Classify.JsonSerialization.Newtonsoft.SingleValueObjectConverter))]
    public abstract class SingleValueObject<TValueType> : ValueObject, ISingleValueObject
    {
        protected SingleValueObject(TValueType value, string classificationType)
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