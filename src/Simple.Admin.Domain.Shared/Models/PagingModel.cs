namespace Simple.Admin.Domain.Shared.Models
{
    public class PagingModel<T> where T : new()
    {
        /// <summary>
        /// 分页后数据
        /// </summary>
        public IEnumerable<T>? Rows { get; set; }

        /// <summary>
        /// 总条目
        /// </summary>
        public int Total { get; set; }

        public PagingModel()
        { }

        public PagingModel(int total, IList<T>? rows)
        {
            Total = total;
            Rows = rows;
        }
    }
}