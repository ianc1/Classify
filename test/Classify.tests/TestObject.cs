namespace Classify.tests
{
    using System;
    using Classify.CommonValueObjects;
    using Classify.CommonValueObjects.Person;

    public class TestObject
    {
        public string TestString { get; set; }
        
        public decimal TestNumber { get; set; }

        public DateTimeOffset TestDateTimeOffset { get; set; }
        
        public PersonalEmailAddress PersonalEmailAddress { get; set; }
        
        public Password Password { get; set; }
    }
}