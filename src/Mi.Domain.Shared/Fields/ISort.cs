namespace Mi.Domain.Shared.Fields
{
    public interface ISort
    {
        /// <summary>
        /// 排序值，越小越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}