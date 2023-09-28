namespace Mi.Domain.Entities.Field
{
    public interface IParentId<T>
    {
        /// <summary>
        /// 父级Id
        /// </summary>
        T ParentId { get; set; }
    }
}