namespace Mi.Domain.Entities.Field
{
    public interface IChildren<T>
    {
        /// <summary>
        /// 子集
        /// </summary>
        T? Children { get; set; }
    }
}