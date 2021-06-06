namespace Classify.tests.BaseValueObjects
{
    using System;
    using System.Collections.Generic;
    using Classify.BaseValueObjects;
    using Classify.CommonValueObjects;
    using Classify.CommonValueObjects.Person;
    using Classify.JsonSerialization.Microsoft;

    public class TestValueObject : ValueObject
    {
        public string NativeString { get; set; }
        
        public decimal? NativeDecimal { get; set; }
        
        public bool? NativeBool { get; set; }

        public DateTimeOffset? NativeDateTimeOffset { get; set; }
        
        public Uri NativeUri { get; set; }
        
        public PersonalEmailAddress EmailAddress { get; set; }
        
        public ApiBaseAddress ApiBaseAddress { get; set; }
        
        public ApiKey ApiKey { get; set; }
        
        public StartTime StartTime { get; set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NativeString;
            yield return NativeDecimal;
            yield return NativeBool;
            yield return NativeDateTimeOffset;
            yield return NativeUri;
            yield return EmailAddress;
            yield return ApiBaseAddress;
            yield return ApiKey;
            yield return StartTime;
        }
    }
    
    [System.Text.Json.Serialization.JsonConverter(typeof(SingleValueObjectConverter))] // Todo - Replace with interface converter when supported.
    public class StartTime : SingleValueObject<DateTime>
    {
        public StartTime(DateTime value)
            : base(value, ClassificationTypes.Public) {}

        public override object SerializeObject() => Value.ToString("o");
    }
}