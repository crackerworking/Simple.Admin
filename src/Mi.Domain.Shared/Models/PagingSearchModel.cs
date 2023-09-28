namespace Mi.Domain.Shared.Models
{
    public class PagingSearchModel
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }

    public interface IPagingSearch
    {
        int Page { get; set; }
        int Size { get; set; }
    }
}