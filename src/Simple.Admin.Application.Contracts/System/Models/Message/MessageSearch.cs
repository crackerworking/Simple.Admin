using Simple.Admin.Domain.Shared.Models;

namespace Simple.Admin.Application.Contracts.System.Models.Message
{
    public class MessageSearch : PagingSearchModel
    {
        public long? No { get; set; }

        public string? Vague { get; set; }

        public string? WriteTime { get; set; }

        public int? Readed { get; set; }
    }
}