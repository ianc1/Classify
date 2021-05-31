namespace Classify.JsonSerialization.Microsoft
{
    public class IncludeSensitiveValueObjectConverter : SimpleValueObjectConverter
    {
        protected override bool IncludeSensitive { get; } = true;
    }
}