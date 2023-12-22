namespace Simple.Admin.Domain.Shared.Models
{
    public class PagingModel<T> : IPagingSearch where T : new()
    {
        /// <summary>
        /// 分页后数据
        /// </summary>
        public IEnumerable<T>? Rows { get; set; }

        /// <summary>
        /// 总条目
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int Size { get; set; }

        public PagingModel()
        { }

        public PagingModel(int total, IList<T>? rows)
        {
            Total = total;
            Rows = rows;
        }

        public PagingModel(int total, IList<T>? rows, IPagingSearch search)
        {
            Total = total;
            Rows = rows;
            Page = search.Page;
            Size = search.Size;
        }
    }
}