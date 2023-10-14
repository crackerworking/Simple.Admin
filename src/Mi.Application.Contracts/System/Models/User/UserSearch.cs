namespace Mi.Application.Contracts.System.Models.User
{
    public class UserSearch : PagingSearchModel
    {
        public string? UserName { get; set; }
    }
}