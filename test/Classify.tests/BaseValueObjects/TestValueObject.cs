namespace Classify.tests.BaseValueObjects
{
    using System;
    using System.Collections.Generic;
    using Classify.BaseValueObjects;
    using Classify.CommonValueObjects;

    public class TestValueObject : ValueObject
    {
        public string NativeString { get; set; }
        
        public decimal NativeDecimal { get; set; }
        
        public bool NativeBool { get; set; }

        public DateTimeOffset NativeDateTimeOffset { get; set; }
        
        public Uri NativeUri { get; set; }
        
        public ApiBaseAddress ApiBaseAddress { get; set; }
        
        public ApiKey ApiKey { get; set; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NativeString;
            yield return NativeDecimal;
            yield return NativeBool;
            yield return NativeDateTimeOffset;
            yield return NativeUri;
            yield return ApiBaseAddress;
            yield return ApiKey;
        }
    }
}