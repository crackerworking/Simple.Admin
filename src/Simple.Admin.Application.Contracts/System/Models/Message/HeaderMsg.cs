namespace Simple.Admin.Application.Contracts.System.Models.Message
{
    public class HeaderMsg
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public IList<HeaderMsgChild>? Children { get; set; }
    }

    public class HeaderMsgChild
    {
        public long Id { get; set; }
        public string? Avatar { get; set; } = "../../../admin/images/msgicon.png";
        public string? Title { get; set; }
        public string? Context { get; set; }
        public string? Form { get; set; } = "系统";
        public string? Time { get; set; }
    }
}