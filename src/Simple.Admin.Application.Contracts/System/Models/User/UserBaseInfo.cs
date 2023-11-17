namespace Simple.Admin.Application.Contracts.System.Models.User
{
    public class UserBaseInfo
    {
        public long UserId { get; set; }

        public string? NickName { get; set; }

        public int Sex { get; set; }

        public string? Signature { get; set; }

        public string? Avatar { get; set; }
    }
}