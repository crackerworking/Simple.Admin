namespace Mi.Domain.Shared.Fields
{
    public interface IChildren<T>
    {
        /// <summary>
        /// 子集
        /// </summary>
        T? Children { get; set; }
    }
}