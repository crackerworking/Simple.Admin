namespace Mi.Domain.Shared.Models
{
    public class PagingModel<T> where T : new()
    {
        public IEnumerable<T>? Rows { get; set; }

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