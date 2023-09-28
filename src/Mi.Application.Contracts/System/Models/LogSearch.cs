namespace Mi.Application.Contracts.System.Models
{
    public class LogSearch : PagingSearchModel
    {
        public string? UserName { get; set; }
        public long? UserId { get; set; }
        public DateTime[]? CreatedOn { get; set; }
        public int Succeed { get; set; }
    }
}