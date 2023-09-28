namespace Mi.Application.Contracts.System.Models
{
    public class MessageSearch : PagingSearchModel
    {
        public long? No { get; set; }

        public string? Vague { get; set; }

        public string? WriteTime { get; set; }

        public int? Readed { get; set; }
    }
}