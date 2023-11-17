namespace Simple.Admin.Domain.Shared.Fields
{
    public interface IParentId<T>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        T ParentId { get; set; }
    }
}