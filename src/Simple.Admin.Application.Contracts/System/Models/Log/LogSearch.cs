using Simple.Admin.Domain.Shared.Models;

namespace Simple.Admin.Application.Contracts.System.Models.Log
{
    public class LogSearch : PagingSearchModel
    {
        public string? UserName { get; set; }
        public long? UserId { get; set; }
        public DateTime[]? CreatedOn { get; set; }
        public int Succeed { get; set; }
    }
}