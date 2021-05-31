namespace Classify.BaseValueObjects
{
    public interface ISingleValueObject
    {
        string ClassificationType { get; }
        
        object SerializeObject();
    }
}