namespace Classify
{
    using Classify.JsonSerialization.Newtonsoft;

    public abstract class SensitiveValueObject<T> : SensitiveValueObject
    {
        public SensitiveValueObject(T value, string classificationType)
            : base(value, classificationType)
        {
            SensitiveValue = value;
        }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public T SensitiveValue { get; }
    }
 
    [Newtonsoft.Json.JsonConverter(typeof(RedactSensitiveJsonConverter))]
    public abstract class SensitiveValueObject
    {
        private readonly object Value;
        
        public SensitiveValueObject(object value, string classificationType)
        {
            Value = value;
            ClassificationType = classificationType;
        }

        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string ClassificationType { get; }

        public override string ToString()
        {
            return $"Redacted {this.GetType().Name}";
        }

        public object GetSensitiveValue()
        {
            return Value;
        }
    }
}
