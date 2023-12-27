namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class UserSearch : PagingSearchModel
    {
        public string? UserName { get; set; }

        public string? NickName { get; set; }

        public int? Sex { get; set; }

        public int? IsEnabled { get; set; }
    }
}