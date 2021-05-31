namespace Classify.BaseValueObjects
{
    public interface ISimpleValueObject
    {
        string ClassificationType { get; }
        
        object SerializeObject();
    }
}