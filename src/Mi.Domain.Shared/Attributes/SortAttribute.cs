namespace Mi.Domain.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SortAttribute : Attribute
    {
        public int Value { get; private set; }

        public SortAttribute(int value)
        {
            this.Value = value;
        }
    }
}