namespace Classify.JsonSerialization.Microsoft
{
    public class IncludeSensitiveValueObjectConverter : SingleValueObjectConverter
    {
        protected override bool IncludeSensitive { get; } = true;
    }
}