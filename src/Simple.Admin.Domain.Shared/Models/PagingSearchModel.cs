namespace Simple.Admin.Domain.Shared.Models
{
    public class PagingSearchModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页n条
        /// </summary>
        public int Size { get; set; } = 10;
    }

    public interface IPagingSearch
    {
        int Page { get; set; }
        int Size { get; set; }
    }
}