namespace Mi.Domain.Shared.Fields
{
    /// <summary>
    /// 主键
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPrimaryKey<T>
    {
        T Id { get; set; }
    }
}