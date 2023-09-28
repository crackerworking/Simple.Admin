namespace Mi.Application.Contracts.System.Models
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