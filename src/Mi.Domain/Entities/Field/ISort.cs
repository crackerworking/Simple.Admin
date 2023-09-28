namespace Mi.Domain.Entities.Field
{
    public interface ISort
    {
        /// <summary>
        /// 排序值，越小越靠前
        /// </summary>
        public int Sort { get; set; }
    }
}