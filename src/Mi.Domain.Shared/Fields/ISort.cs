namespace Mi.Domain.Shared.Fields
{
    /// <summary>
    /// 排序
    /// </summary>
    public interface ISort
    {
        /// <summary>
        /// 排序值，越小越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}