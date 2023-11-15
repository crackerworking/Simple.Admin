namespace Mi.Domain.Shared.Attributes
{
    /// <summary>
    /// 排序，目前用于管道配置顺序
    /// </summary>
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